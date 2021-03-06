﻿using AnimeBrowser.BL.Helpers;
using AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces;
using AnimeBrowser.BL.Validators.SecondaryValidators;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
using AnimeBrowser.Common.Models.ResponseModels.SecondaryModels;
using AnimeBrowser.Data.Converters.SecondaryConverters;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Entities.Identity;
using AnimeBrowser.Data.Interfaces.Read.IdentityInterfaces;
using AnimeBrowser.Data.Interfaces.Read.MainInterfaces;
using AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces;
using AnimeBrowser.Data.Interfaces.Write.SecondaryInterfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write.SecondaryHandlers
{
    public class EpisodeRatingEditingHandler : IEpisodeRatingEditing
    {
        private readonly ILogger logger = Log.ForContext<EpisodeRatingEditingHandler>();
        private readonly IEpisodeRatingRead episodeRatingReadRepo;
        private readonly IEpisodeRead episodeReadRepo;
        private readonly IUserRead userReadRepo;
        private readonly IEpisodeRatingWrite episodeRatingWriteRepo;

        public EpisodeRatingEditingHandler(IEpisodeRatingRead episodeRatingReadRepo, IEpisodeRead episodeReadRepo, IUserRead userReadRepo, IEpisodeRatingWrite episodeRatingWriteRepo)
        {
            this.episodeRatingReadRepo = episodeRatingReadRepo;
            this.episodeReadRepo = episodeReadRepo;
            this.userReadRepo = userReadRepo;
            this.episodeRatingWriteRepo = episodeRatingWriteRepo;
        }

        public async Task<EpisodeRatingEditingResponseModel> EditEpisodeRating(long id, EpisodeRatingEditingRequestModel episodeRatingRequestModel, IEnumerable<Claim>? claims)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with {nameof(id)}: [{id}], {nameof(episodeRatingRequestModel)}: [{episodeRatingRequestModel}].");

                if (id != episodeRatingRequestModel?.Id)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                       description: $"The parameter [{nameof(id)}] and [{nameof(episodeRatingRequestModel)}.{nameof(episodeRatingRequestModel.Id)}] properties should have the same value, but they are different!",
                       source: nameof(id), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var mismatchEx = new MismatchingIdException(error, "The given id and the model's id are not matching!");
                    throw mismatchEx;
                }

                var episodeRating = await episodeRatingReadRepo.GetEpisodeRatingById(id);
                if (episodeRating == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                        description: $"No {nameof(EpisodeRating)} object was found with the given id [{id}]!",
                        source: nameof(id), title: ErrorCodes.EmptyObject.GetDescription()
                    );
                    var notExistingEpisodeRatingEx = new NotFoundObjectException<EpisodeRating>(error, $"There is no {nameof(EpisodeRating)} with given id: [{id}].");
                    throw notExistingEpisodeRatingEx;
                }

                var userId = claims?.SingleOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase));
                if (userId == null || !episodeRatingRequestModel.UserId.Equals(userId.Value, StringComparison.OrdinalIgnoreCase) || !episodeRating.UserId.Equals(userId.Value, StringComparison.OrdinalIgnoreCase))
                {
                    throw new UnauthorizedAccessException("Given model's and token's user id are not matching!");
                }

                if (episodeRatingRequestModel.EpisodeId != episodeRating.EpisodeId)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                      description: $"The parameter [{nameof(EpisodeRating)}.{nameof(EpisodeRating.EpisodeId)}] and [{nameof(episodeRatingRequestModel)}.{nameof(episodeRatingRequestModel.EpisodeId)}] properties should have the same value, but they are different!",
                      source: nameof(episodeRatingRequestModel.EpisodeId), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var mismatchEx = new MismatchingIdException(error, "The given id and the model's id are not matching!");
                    throw mismatchEx;
                }

                var episode = await episodeReadRepo.GetEpisodeById(episodeRatingRequestModel.EpisodeId);
                if (episode == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyProperty.GetIntValueAsString(),
                         description: $"No {nameof(Episode)} object was found with the {nameof(EpisodeRating)}'s {nameof(episodeRatingRequestModel.EpisodeId)} [{episodeRatingRequestModel.EpisodeId}]!",
                         source: nameof(episodeRatingRequestModel.EpisodeId), title: ErrorCodes.EmptyProperty.GetDescription()
                     );
                    var notExistingEpisodeEx = new NotFoundObjectException<Episode>(error, $"There is no {nameof(Episode)} object that was given in {nameof(episodeRatingRequestModel.EpisodeId)} property.");
                    throw notExistingEpisodeEx;
                }

                var user = await userReadRepo.GetUserById(episodeRatingRequestModel.UserId);
                if (user == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyProperty.GetIntValueAsString(),
                         description: $"No {nameof(User)} object was found with the {nameof(EpisodeRating)}'s {nameof(episodeRatingRequestModel.UserId)} [{episodeRatingRequestModel.UserId}]!",
                         source: nameof(episodeRatingRequestModel.UserId), title: ErrorCodes.EmptyProperty.GetDescription()
                     );
                    var notExistingUserEx = new NotFoundObjectException<User>(error, $"There is no {nameof(User)} object that was given in {nameof(episodeRatingRequestModel.UserId)} property.");
                    throw notExistingUserEx;
                }

                var validator = new EpisodeRatingEditingValidator();
                var validationResult = await validator.ValidateAsync(episodeRatingRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(EpisodeRatingEditingRequestModel)}].{Environment.NewLine}Validation errors: [{string.Join(", ", errorList)}].");
                }

                var rEpisodeRating = episodeRatingRequestModel.ToEpisodeRating();

                episodeRating.Rating = rEpisodeRating.Rating;
                episodeRating.Message = rEpisodeRating.Message;
                episodeRating.IsAnimeInfoActive = episode.IsAnimeInfoActive;
                episodeRating.IsSeasonActive = episode.IsSeasonActive;
                episodeRating.IsEpisodeActive = episode.IsActive;

                episodeRating = await episodeRatingWriteRepo.UpdateEpisodeRating(episodeRating);
                EpisodeRatingEditingResponseModel responseModel = episodeRating.ToEditingResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(EpisodeRatingEditingResponseModel)}.{nameof(EpisodeRatingEditingResponseModel.Id)}: [{responseModel.Id}].");

                return responseModel;
            }
            catch (MismatchingIdException mismatchEx)
            {
                logger.Warning(mismatchEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{mismatchEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<EpisodeRating> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (NotFoundObjectException<Episode> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (NotFoundObjectException<User> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
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
