using AnimeBrowser.BL.Helpers;
using AnimeBrowser.BL.Interfaces.DateTimeProviders;
using AnimeBrowser.BL.Interfaces.Write.MainInterfaces;
using AnimeBrowser.BL.Validators.MainValidators;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels.MainModels;
using AnimeBrowser.Common.Models.ResponseModels.MainModels;
using AnimeBrowser.Data.Converters.MainConverters;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.MainInterfaces;
using AnimeBrowser.Data.Interfaces.Write.MainInterfaces;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write.MainHandlers
{
    public class EpisodeCreationHandler : IEpisodeCreation
    {
        private readonly ILogger logger = Log.ForContext<EpisodeCreationHandler>();
        private readonly IDateTime dateTimeProvider;
        private readonly IEpisodeRead episodeReadRepo;
        private readonly IEpisodeWrite episodeWriteRepo;
        private readonly ISeasonRead seasonReadRepo;

        public EpisodeCreationHandler(IDateTime dateTimeProvider, IEpisodeRead episodeReadRepo, IEpisodeWrite episodeWriteRepo, ISeasonRead seasonReadRepo)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.episodeReadRepo = episodeReadRepo;
            this.episodeWriteRepo = episodeWriteRepo;
            this.seasonReadRepo = seasonReadRepo;
        }

        public async Task<EpisodeCreationResponseModel> CreateEpisode(EpisodeCreationRequestModel episodeRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(episodeRequestModel)}: [{episodeRequestModel}].");
                if (episodeRequestModel == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(), description: $"The {nameof(EpisodeCreationRequestModel)} object is empty!",
                        source: nameof(episodeRequestModel), title: ErrorCodes.EmptyObject.GetDescription());
                    throw new EmptyObjectException<EpisodeCreationRequestModel>(error, $"The given [{nameof(EpisodeCreationRequestModel)}] object is empty!");
                }

                var season = await seasonReadRepo.GetSeasonById(episodeRequestModel.SeasonId);
                if (season == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                        description: $"No {nameof(Season)} object was found with the given id [{episodeRequestModel.SeasonId}]!",
                        source: nameof(episodeRequestModel.SeasonId), title: ErrorCodes.EmptyObject.GetDescription()
                    );
                    var notExistingSeasonEx = new NotFoundObjectException<Season>(error, $"There is no {nameof(Season)} with given id: [{episodeRequestModel.SeasonId}].");
                    throw notExistingSeasonEx;
                }
                if (season.AnimeInfoId != episodeRequestModel.AnimeInfoId)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                        description: $"The given [{nameof(episodeRequestModel.AnimeInfoId)}] not matches the given {nameof(Season)}'s {nameof(Season.AnimeInfoId)}!",
                        source: nameof(episodeRequestModel.AnimeInfoId), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var mismatchEx = new MismatchingIdException(error, error.Description);
                    throw mismatchEx;
                }

                var episodeValidator = new EpisodeCreationValidator(dateTimeProvider, season.StartDate, season.EndDate);
                var validationResult = await episodeValidator.ValidateAsync(episodeRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    var valEx = new ValidationException(errorList, $"Validation error in [{nameof(EpisodeCreationRequestModel)}].{Environment.NewLine}Validation errors: [{string.Join(", ", errorList)}].");
                    throw valEx;
                }

                var isEpisodeWithSameNumberExists = episodeReadRepo.IsEpisodeWithEpisodeNumberExists(episodeRequestModel.SeasonId, episodeRequestModel.EpisodeNumber);
                if (isEpisodeWithSameNumberExists)
                {
                    var error = new ErrorModel(code: ErrorCodes.NotUniqueProperty.GetIntValueAsString(), description: $"Another {nameof(Episode)} can be found in the same {nameof(Season)} [{episodeRequestModel.SeasonId}] " +
                        $"with the same {nameof(episodeRequestModel.EpisodeNumber)} [{episodeRequestModel.EpisodeNumber}].",
                        source: nameof(episodeRequestModel.EpisodeNumber), title: ErrorCodes.NotUniqueProperty.GetDescription());
                    var alreadyExistingEx = new AlreadyExistingObjectException<Episode>(error, $"There is already an {nameof(Episode)} in the same {nameof(Season)} with the same {nameof(Episode.EpisodeNumber)} value.");
                    throw alreadyExistingEx;
                }

                var episode = await episodeWriteRepo.CreateEpisode(episodeRequestModel.ToEpisode());
                var responseModel = episode.ToCreationResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(EpisodeCreationResponseModel)}.{nameof(EpisodeCreationResponseModel.Id)}: [{responseModel.Id}].");

                return responseModel;
            }
            catch (EmptyObjectException<EpisodeCreationRequestModel> emptyObjEx)
            {
                logger.Warning(emptyObjEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{emptyObjEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<Season> notFoundEx)
            {
                logger.Warning(notFoundEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{notFoundEx.Message}].");
                throw;
            }
            catch (MismatchingIdException mismatchEx)
            {
                logger.Warning(mismatchEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{mismatchEx.Message}].");
                throw;
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                throw;
            }
            catch (AlreadyExistingObjectException<Episode> alreadyExistingEx)
            {
                logger.Warning(alreadyExistingEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{alreadyExistingEx.Message}].");
                throw;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
        }
    }
}
