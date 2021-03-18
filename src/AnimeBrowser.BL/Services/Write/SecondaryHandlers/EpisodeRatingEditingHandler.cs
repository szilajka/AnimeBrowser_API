using AnimeBrowser.BL.Helpers;
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

        public async Task<EpisodeRatingEditingResponseModel> EditEpisodeRating(long id, EpisodeRatingEditingRequestModel episodeRatingRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with {nameof(id)}: [{id}], {nameof(episodeRatingRequestModel)}: [{episodeRatingRequestModel}].");

                if (id != episodeRatingRequestModel?.Id)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                       description: $"The parameter [{nameof(id)}] and [{nameof(episodeRatingRequestModel)}.{nameof(EpisodeRatingEditingRequestModel.Id)}] properties should have the same value, but they are different!",
                       source: nameof(id), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var mismatchEx = new MismatchingIdException(error, "The given id and the model's id are not matching!");
                    throw mismatchEx;
                }

                var episodeRating = await episodeRatingReadRepo.GetEpisodeRatingById(id);
                if (episodeRating == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                        description: $"No {nameof(EpisodeRating)} object was found with the given id [{id}]!",
                        source: nameof(EpisodeRatingEditingRequestModel.Id), title: ErrorCodes.EmptyObject.GetDescription()
                    );
                    var notExistingEpisodeRatingEx = new NotFoundObjectException<EpisodeRating>(error, $"There is no {nameof(EpisodeRating)} with given id: [{id}].");
                    throw notExistingEpisodeRatingEx;
                }

                if (episodeRatingRequestModel.EpisodeId != episodeRating.EpisodeId)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                      description: $"The parameter [{nameof(EpisodeRating)}.{nameof(EpisodeRating.EpisodeId)}] and [{nameof(episodeRatingRequestModel)}.{nameof(EpisodeRatingEditingRequestModel.EpisodeId)}] properties should have the same value, but they are different!",
                      source: nameof(episodeRatingRequestModel.EpisodeId), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var mismatchEx = new MismatchingIdException(error, "The given id and the model's id are not matching!");
                    throw mismatchEx;
                }
                if (!episodeRatingRequestModel.UserId.Equals(episodeRating.UserId, StringComparison.OrdinalIgnoreCase))
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                      description: $"The parameter [{nameof(EpisodeRating)}.{nameof(EpisodeRating.UserId)}] and [{nameof(episodeRatingRequestModel)}.{nameof(EpisodeRatingEditingRequestModel.UserId)}] properties should have the same value, but they are different!",
                      source: nameof(episodeRatingRequestModel.UserId), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var mismatchEx = new MismatchingIdException(error, "The given id and the model's id are not matching!");
                    throw mismatchEx;
                }

                var episode = await episodeReadRepo.GetEpisodeById(episodeRatingRequestModel.EpisodeId);
                if (episode == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyProperty.GetIntValueAsString(),
                         description: $"No {nameof(Episode)} object was found with the {nameof(EpisodeRating)}'s {nameof(EpisodeRatingEditingRequestModel.EpisodeId)} [{episodeRatingRequestModel.EpisodeId}]!",
                         source: nameof(EpisodeRatingEditingRequestModel.EpisodeId), title: ErrorCodes.EmptyProperty.GetDescription()
                     );
                    var notExistingEpisodeEx = new NotFoundObjectException<Episode>(error, $"There is no {nameof(Episode)} object that was given in {nameof(EpisodeRatingEditingRequestModel.EpisodeId)} property.");
                    throw notExistingEpisodeEx;
                }

                var user = await userReadRepo.GetUserById(episodeRatingRequestModel.UserId);
                if (user == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyProperty.GetIntValueAsString(),
                         description: $"No {nameof(User)} object was found with the {nameof(EpisodeRating)}'s {nameof(EpisodeRatingEditingRequestModel.UserId)} [{episodeRatingRequestModel.UserId}]!",
                         source: nameof(EpisodeRatingEditingRequestModel.UserId), title: ErrorCodes.EmptyProperty.GetDescription()
                     );
                    var notExistingUserEx = new NotFoundObjectException<User>(error, $"There is no {nameof(User)} object that was given in {nameof(EpisodeRatingEditingRequestModel.UserId)} property.");
                    throw notExistingUserEx;
                }

                var validator = new EpisodeRatingEditingValidator();
                var validationResult = await validator.ValidateAsync(episodeRatingRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(EpisodeRatingEditingRequestModel)}].{Environment.NewLine}Validation errors:[{string.Join(", ", errorList)}].");
                }

                var rEpisodeRating = episodeRatingRequestModel.ToEpisodeRating();

                episodeRating.Rating = rEpisodeRating.Rating;
                episodeRating.Message = rEpisodeRating.Message;

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
