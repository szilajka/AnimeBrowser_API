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
    public class SeasonRatingEditingHandler : ISeasonRatingEditing
    {
        private readonly ILogger logger = Log.ForContext<SeasonRatingEditingHandler>();
        private readonly ISeasonRatingRead seasonRatingReadRepo;
        private readonly ISeasonRead seasonReadRepo;
        private readonly IUserRead userReadRepo;
        private readonly ISeasonRatingWrite seasonRatingWriteRepo;

        public SeasonRatingEditingHandler(ISeasonRatingRead seasonRatingReadRepo, ISeasonRead seasonReadRepo, IUserRead userReadRepo, ISeasonRatingWrite seasonRatingWriteRepo)
        {
            this.seasonRatingReadRepo = seasonRatingReadRepo;
            this.seasonReadRepo = seasonReadRepo;
            this.userReadRepo = userReadRepo;
            this.seasonRatingWriteRepo = seasonRatingWriteRepo;
        }

        public async Task<SeasonRatingEditingResponseModel> EditSeasonRating(long id, SeasonRatingEditingRequestModel seasonRatingRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with {nameof(id)}: [{id}], {nameof(seasonRatingRequestModel)}: [{seasonRatingRequestModel}].");

                if (id != seasonRatingRequestModel?.Id)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                       description: $"The parameter [{nameof(id)}] and [{nameof(seasonRatingRequestModel)}.{nameof(seasonRatingRequestModel.Id)}] properties should have the same value, but they are different!",
                       source: nameof(id), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var mismatchEx = new MismatchingIdException(error, "The given id and the model's id are not matching!");
                    throw mismatchEx;
                }

                var seasonRating = await seasonRatingReadRepo.GetSeasonRatingById(id);
                if (seasonRating == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                        description: $"No {nameof(SeasonRating)} object was found with the given id [{id}]!",
                        source: nameof(id), title: ErrorCodes.EmptyObject.GetDescription()
                    );
                    var notExistingSeasonRatingEx = new NotFoundObjectException<SeasonRating>(error, $"There is no {nameof(SeasonRating)} with given id: [{id}].");
                    throw notExistingSeasonRatingEx;
                }

                if (seasonRatingRequestModel.SeasonId != seasonRating.SeasonId)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                      description: $"The parameter [{nameof(SeasonRating)}.{nameof(SeasonRating.SeasonId)}] and [{nameof(seasonRatingRequestModel)}.{nameof(seasonRatingRequestModel.SeasonId)}] properties should have the same value, but they are different!",
                      source: nameof(seasonRatingRequestModel.SeasonId), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var mismatchEx = new MismatchingIdException(error, "The given id and the model's id are not matching!");
                    throw mismatchEx;
                }
                if (!seasonRatingRequestModel.UserId.Equals(seasonRating.UserId, StringComparison.OrdinalIgnoreCase))
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                      description: $"The parameter [{nameof(SeasonRating)}.{nameof(SeasonRating.UserId)}] and [{nameof(seasonRatingRequestModel)}.{nameof(seasonRatingRequestModel.UserId)}] properties should have the same value, but they are different!",
                      source: nameof(seasonRatingRequestModel.UserId), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var mismatchEx = new MismatchingIdException(error, "The given id and the model's id are not matching!");
                    throw mismatchEx;
                }

                var season = await seasonReadRepo.GetSeasonById(seasonRatingRequestModel.SeasonId);
                if (season == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyProperty.GetIntValueAsString(),
                         description: $"No {nameof(Season)} object was found with the {nameof(SeasonRating)}'s {nameof(seasonRatingRequestModel.SeasonId)} [{seasonRatingRequestModel.SeasonId}]!",
                         source: nameof(seasonRatingRequestModel.SeasonId), title: ErrorCodes.EmptyProperty.GetDescription()
                     );
                    var notExistingSeasonEx = new NotFoundObjectException<Season>(error, $"There is no {nameof(Season)} object that was given in {nameof(seasonRatingRequestModel.SeasonId)} property.");
                    throw notExistingSeasonEx;
                }

                var user = await userReadRepo.GetUserById(seasonRatingRequestModel.UserId);
                if (user == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyProperty.GetIntValueAsString(),
                         description: $"No {nameof(User)} object was found with the {nameof(SeasonRating)}'s {nameof(SeasonRatingEditingRequestModel.UserId)} [{seasonRatingRequestModel.UserId}]!",
                         source: nameof(seasonRatingRequestModel.UserId), title: ErrorCodes.EmptyProperty.GetDescription()
                     );
                    var notExistingUserEx = new NotFoundObjectException<User>(error, $"There is no {nameof(User)} object that was given in {nameof(seasonRatingRequestModel.UserId)} property.");
                    throw notExistingUserEx;
                }

                var validator = new SeasonRatingEditingValidator();
                var validationResult = await validator.ValidateAsync(seasonRatingRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(SeasonRatingEditingRequestModel)}].{Environment.NewLine}Validation errors: [{string.Join(", ", errorList)}].");
                }

                var rSeasonRating = seasonRatingRequestModel.ToSeasonRating();

                seasonRating.Rating = rSeasonRating.Rating;
                seasonRating.Message = rSeasonRating.Message;

                seasonRating = await seasonRatingWriteRepo.UpdateSeasonRating(seasonRating);
                SeasonRatingEditingResponseModel responseModel = seasonRating.ToEditingResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(SeasonRatingEditingResponseModel)}.{nameof(SeasonRatingEditingResponseModel.Id)}: [{responseModel.Id}].");

                return responseModel;
            }
            catch (MismatchingIdException mismatchEx)
            {
                logger.Warning(mismatchEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{mismatchEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<SeasonRating> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (NotFoundObjectException<Season> ex)
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
