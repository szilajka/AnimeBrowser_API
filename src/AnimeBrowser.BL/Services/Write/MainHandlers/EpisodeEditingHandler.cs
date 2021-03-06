﻿using AnimeBrowser.BL.Helpers;
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
    public class EpisodeEditingHandler : IEpisodeEditing
    {
        private readonly ILogger logger = Log.ForContext<EpisodeEditingHandler>();
        private readonly IDateTime dateTimeProvider;
        private readonly IEpisodeRead episodeReadRepo;
        private readonly IEpisodeWrite episodeWriteRepo;
        private readonly ISeasonRead seasonReadRepo;

        public EpisodeEditingHandler(IDateTime dateTimeProvider, IEpisodeRead episodeReadRepo, IEpisodeWrite episodeWriteRepo, ISeasonRead seasonReadRepo)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.episodeReadRepo = episodeReadRepo;
            this.episodeWriteRepo = episodeWriteRepo;
            this.seasonReadRepo = seasonReadRepo;
        }

        public async Task<EpisodeEditingResponseModel> EditEpisode(long id, EpisodeEditingRequestModel episodeRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with {nameof(id)}: [{id}], {nameof(episodeRequestModel)}: [{episodeRequestModel}].");

                if (id != episodeRequestModel?.Id)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                       description: $"The parameter [{nameof(id)}] and [{nameof(episodeRequestModel)}.{nameof(episodeRequestModel.Id)}] properties should have the same value, but they are different!",
                       source: nameof(id), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var mismatchEx = new MismatchingIdException(error, "The given id and the model's id are not matching!");
                    throw mismatchEx;
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

                var validator = new EpisodeEditingValidator(dateTimeProvider, season.StartDate, season.EndDate);
                var validationResult = await validator.ValidateAsync(episodeRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(EpisodeEditingRequestModel)}].{Environment.NewLine}Validation errors: [{string.Join(", ", errorList)}].");
                }

                var episode = await episodeReadRepo.GetEpisodeById(id);
                if (episode == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                        description: $"No {nameof(Episode)} object was found with the given id [{id}]!",
                        source: nameof(EpisodeEditingRequestModel.Id), title: ErrorCodes.EmptyObject.GetDescription()
                    );
                    var notExistingEpisodeEx = new NotFoundObjectException<Episode>(error, $"There is no {nameof(Episode)} with given id: [{id}].");
                    throw notExistingEpisodeEx;
                }

                var isEpisodeWithSameNumberExists = episodeReadRepo.IsEpisodeWithEpisodeNumberExists(seasonId: episodeRequestModel.SeasonId, episodeNumber: episodeRequestModel.EpisodeNumber);
                if (isEpisodeWithSameNumberExists && episodeRequestModel.EpisodeNumber != episode.EpisodeNumber)
                {
                    var error = new ErrorModel(code: ErrorCodes.NotUniqueProperty.GetIntValueAsString(), description: $"Another {nameof(Episode)} can be found in the same {nameof(Season)} [{episodeRequestModel.SeasonId}] " +
                        $"with the same {nameof(episodeRequestModel.EpisodeNumber)} [{episodeRequestModel.EpisodeNumber}].",
                        source: nameof(episodeRequestModel.EpisodeNumber), title: ErrorCodes.NotUniqueProperty.GetDescription());
                    var alreadyExistingEx = new AlreadyExistingObjectException<Episode>(error, $"There is already an {nameof(Episode)} in the same {nameof(Season)} with the same {nameof(Episode.EpisodeNumber)} value.");
                    throw alreadyExistingEx;
                }

                var rEpisode = episodeRequestModel.ToEpisode();
                episode.UpdateEpisodeWithOtherEpisode(rEpisode);

                episode = await episodeWriteRepo.UpdateEpisode(episode);
                EpisodeEditingResponseModel responseModel = episode.ToEditingResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. result: [{responseModel}].");

                return responseModel;
            }
            catch (MismatchingIdException mismatchEx)
            {
                logger.Warning(mismatchEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{mismatchEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<Season> notFoundEx)
            {
                logger.Warning(notFoundEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{notFoundEx.Message}].");
                throw;
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<Episode> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (AlreadyExistingObjectException<Episode> alreadyEx)
            {
                logger.Warning(alreadyEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{alreadyEx.Message}].");
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
