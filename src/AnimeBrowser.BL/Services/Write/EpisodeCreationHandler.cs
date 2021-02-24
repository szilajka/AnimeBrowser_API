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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write
{
    public class EpisodeCreationHandler : IEpisodeCreation
    {
        private readonly ILogger logger = Log.ForContext<EpisodeCreationHandler>();
        private readonly IDateTime dateTimeProvider;
        private readonly IEpisodeRead episodeReadRepo;
        private readonly IEpisodeWrite episodeWriteRepo;

        public EpisodeCreationHandler(IDateTime dateTimeProvider, IEpisodeRead episodeReadRepo, IEpisodeWrite episodeWriteRepo)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.episodeReadRepo = episodeReadRepo;
            this.episodeWriteRepo = episodeWriteRepo;
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

                var episodeValidator = new EpisodeCreationValidator(dateTimeProvider);
                var validationResult = await episodeValidator.ValidateAsync(episodeRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    var valEx = new ValidationException(errorList, $"Validation error in [{nameof(EpisodeCreationRequestModel)}].{Environment.NewLine}Validation errors:[{string.Join(", ", errorList)}].");
                    throw valEx;
                }

                var isSeasonAndAnimeInfoExists = await episodeReadRepo.IsSeasonAndAnimeInfoExistsAndReferences(seasonId: episodeRequestModel.SeasonId, animeInfoId: episodeRequestModel.AnimeInfoId);
                if (!isSeasonAndAnimeInfoExists)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                        description: $"There is no {nameof(AnimeInfo)} and/or {nameof(Season)} with given ids, " +
                                        $"and/or the [{nameof(Season)}.{nameof(Season.AnimeInfoId)}] not matches the " +
                                        $"[{nameof(EpisodeCreationRequestModel)}.{nameof(EpisodeCreationRequestModel.AnimeInfoId)}] property.",
                        source: $"[{nameof(episodeRequestModel.SeasonId)}, {nameof(episodeRequestModel.AnimeInfoId)}]", title: ErrorCodes.MismatchingProperty.GetDescription());
                    var mismatchEx = new MismatchingIdException(error, error.Description);
                    throw mismatchEx;
                }
                var isEpisodeWithSameNumberExists = episodeReadRepo.IsEpisodeWithEpisodeNumberExists(episodeRequestModel.SeasonId, episodeRequestModel.EpisodeNumber);
                if (isEpisodeWithSameNumberExists)
                {
                    var error = new ErrorModel(code: ErrorCodes.NotUniqueProperty.GetIntValueAsString(), description: $"Another {nameof(Episode)} can be found in the same {nameof(Season)} [{episodeRequestModel.SeasonId}] " +
                        $"with the same {nameof(EpisodeCreationRequestModel.EpisodeNumber)} [{episodeRequestModel.EpisodeNumber}].",
                        source: nameof(EpisodeCreationRequestModel.EpisodeNumber), title: ErrorCodes.NotUniqueProperty.GetDescription());
                    var alreadyExistingEx = new AlreadyExistingObjectException<Episode>(error);
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
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                throw;
            }
            catch (MismatchingIdException mismatchEx)
            {
                logger.Warning(mismatchEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{mismatchEx.Message}].");
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
