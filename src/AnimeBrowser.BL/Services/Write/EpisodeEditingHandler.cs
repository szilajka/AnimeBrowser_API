using AnimeBrowser.BL.Helpers;
using AnimeBrowser.BL.Interfaces.DateTimeProviders;
using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Validators;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Common.Models.ResponseModels;
using AnimeBrowser.Data.Converters;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read;
using AnimeBrowser.Data.Interfaces.Write;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write
{
    public class EpisodeEditingHandler : IEpisodeEditing
    {
        private readonly ILogger logger = Log.ForContext<EpisodeEditingHandler>();
        private readonly IDateTime dateTimeProvider;
        private readonly IEpisodeRead episodeReadRepo;
        private readonly IEpisodeWrite episodeWriteRepo;

        public EpisodeEditingHandler(IDateTime dateTimeProvider, IEpisodeRead episodeReadRepo, IEpisodeWrite episodeWriteRepo)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.episodeReadRepo = episodeReadRepo;
            this.episodeWriteRepo = episodeWriteRepo;
        }

        public async Task<EpisodeEditingResponseModel> EditEpisode(long id, EpisodeEditingRequestModel episodeRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with requestModel: [{episodeRequestModel}].");

                if (id != episodeRequestModel?.Id)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                       description: $"The parameter [{nameof(id)}] and [{nameof(episodeRequestModel)}.{nameof(EpisodeEditingRequestModel.Id)}] properties should have the same value, but they are different!",
                       source: nameof(id), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var argEx = new MismatchingIdException(error, "The given id and the model's id are not matching!");
                    logger.Warning(argEx, $"Id mismatch in property and parameter.");
                    throw argEx;
                }

                var validator = new EpisodeEditingValidator(dateTimeProvider);
                var validationResult = await validator.ValidateAsync(episodeRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(EpisodeEditingRequestModel)}].");
                }

                var episode = await episodeReadRepo.GetEpisodeById(id);
                if (episode == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                        description: $"No {nameof(Episode)} object was found with the given id [{id}]!",
                        source: nameof(EpisodeEditingRequestModel.Id), title: ErrorCodes.EmptyObject.GetDescription()
                    );
                    var notExistingEpisodeEx = new NotFoundObjectException<EpisodeEditingRequestModel>(error, $"There is no {nameof(Episode)} with given id: [{id}].");
                    logger.Warning(notExistingEpisodeEx, notExistingEpisodeEx.Message);
                    throw notExistingEpisodeEx;
                }

                var isSeasonAndAnimeInfoExists = await episodeReadRepo.IsSeasonAndAnimeInfoExistsAndReferences(seasonId: episodeRequestModel.SeasonId, animeInfoId: episodeRequestModel.AnimeInfoId);
                if (!isSeasonAndAnimeInfoExists)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                        description: $"There is no {nameof(AnimeInfo)} and/or {nameof(Season)} with given ids, " +
                                        $"and/or the [{nameof(Season)}.{nameof(Season.AnimeInfoId)}] not matches the " +
                                        $"[{nameof(EpisodeEditingRequestModel)}.{nameof(EpisodeEditingRequestModel.AnimeInfoId)}] property.",
                        source: $"[{nameof(episodeRequestModel.SeasonId)}, {nameof(episodeRequestModel.AnimeInfoId)}]", title: ErrorCodes.MismatchingProperty.GetDescription());
                    var mismatchEx = new MismatchingIdException(error, error.Description);
                    logger.Warning(mismatchEx, mismatchEx.Message);
                    throw mismatchEx;
                }
                var isEpisodeWithSameNumberExists = episodeReadRepo.IsEpisodeWithEpisodeNumberExists(seasonId: episodeRequestModel.SeasonId, episodeNumber: episodeRequestModel.EpisodeNumber);
                if (isEpisodeWithSameNumberExists && episodeRequestModel.EpisodeNumber != episode.EpisodeNumber)
                {
                    var error = new ErrorModel(code: ErrorCodes.NotUniqueProperty.GetIntValueAsString(), description: $"Another {nameof(Episode)} can be found in the same {nameof(Season)} [{episodeRequestModel.SeasonId}] " +
                        $"with the same {nameof(EpisodeEditingRequestModel.EpisodeNumber)} [{episodeRequestModel.EpisodeNumber}].",
                        source: nameof(EpisodeEditingRequestModel.EpisodeNumber), title: ErrorCodes.NotUniqueProperty.GetDescription());
                    var alreadyExistingEx = new AlreadyExistingObjectException<Episode>(error);
                    throw alreadyExistingEx;
                }

                var rEpisode = episodeRequestModel.ToEpisode();
                episode.UpdateEpisodeWithOtherEpisode(rEpisode);

                episode = await episodeWriteRepo.UpdateEpisode(episode);
                EpisodeEditingResponseModel responseModel = episode.ToEditingResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. result: [{responseModel}].");

                return responseModel;
            }
            catch (ValidationException valEx)
            {
                logger.Warning($"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<EpisodeEditingRequestModel> ex)
            {
                logger.Warning($"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
        }
    }
}
