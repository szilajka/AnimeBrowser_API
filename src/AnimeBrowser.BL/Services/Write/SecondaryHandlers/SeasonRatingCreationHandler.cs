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
    public class SeasonRatingCreationHandler : ISeasonRatingCreation
    {
        private readonly ILogger logger = Log.ForContext<SeasonRatingCreationHandler>();
        private readonly ISeasonRead seasonReadRepo;
        private readonly IUserRead userReadRepo;
        private readonly ISeasonRatingRead seasonRatingReadRepo;
        private readonly ISeasonRatingWrite seasonRatingWriteRepo;

        public SeasonRatingCreationHandler(ISeasonRead seasonReadRepo, IUserRead userReadRepo, ISeasonRatingRead seasonRatingReadRepo, ISeasonRatingWrite seasonRatingWriteRepo)
        {
            this.seasonReadRepo = seasonReadRepo;
            this.userReadRepo = userReadRepo;
            this.seasonRatingReadRepo = seasonRatingReadRepo;
            this.seasonRatingWriteRepo = seasonRatingWriteRepo;
        }

        public async Task<SeasonRatingCreationResponseModel> CreateSeasonRating(SeasonRatingCreationRequestModel seasonRatingRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonRatingRequestModel)}: [{seasonRatingRequestModel}].");
                if (seasonRatingRequestModel == null)
                {
                    var errorModel = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(), description: $"The {nameof(SeasonRatingCreationRequestModel)} object is empty!",
                        source: nameof(seasonRatingRequestModel), title: ErrorCodes.EmptyObject.GetDescription());
                    throw new EmptyObjectException<SeasonRatingCreationRequestModel>(errorModel, $"The given {nameof(SeasonRatingCreationRequestModel)} object is empty!");
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
                         description: $"No {nameof(User)} object was found with the {nameof(SeasonRating)}'s {nameof(seasonRatingRequestModel.UserId)} [{seasonRatingRequestModel.UserId}]!",
                         source: nameof(seasonRatingRequestModel.UserId), title: ErrorCodes.EmptyProperty.GetDescription()
                     );
                    var notExistingUserEx = new NotFoundObjectException<User>(error, $"There is no {nameof(User)} object that was given in {nameof(seasonRatingRequestModel.UserId)} property.");
                    throw notExistingUserEx;
                }

                var validator = new SeasonRatingCreationValidator();
                var validationResult = await validator.ValidateAsync(seasonRatingRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(SeasonRatingCreationRequestModel)}].{Environment.NewLine}Validation errors: [{string.Join(", ", errorList)}].");
                }

                var existingSeason = seasonRatingReadRepo.GetSeasonRatingBySeasonAndUserId(seasonRatingRequestModel.SeasonId, seasonRatingRequestModel.UserId);
                if (existingSeason != null)
                {
                    var error = new ErrorModel(code: ErrorCodes.NotUniqueProperty.GetIntValueAsString(), description: $"Another {nameof(SeasonRating)} can be found with the same {nameof(SeasonRating.SeasonId)} [{seasonRatingRequestModel.SeasonId}] " +
                       $"and the same {nameof(seasonRatingRequestModel.UserId)} [{seasonRatingRequestModel.UserId}].",
                       source: $"[{nameof(seasonRatingRequestModel.SeasonId)}, {nameof(seasonRatingRequestModel.UserId)}]", title: ErrorCodes.NotUniqueProperty.GetDescription());
                    var alreadyExistingEx = new AlreadyExistingObjectException<SeasonRating>(error, $"There is already a {nameof(SeasonRating)} with the same {nameof(Season)} and the same {nameof(User)} value.");
                    throw alreadyExistingEx;
                }

                var seasonRating = seasonRatingRequestModel.ToSeasonRating();
                seasonRating.IsAnimeInfoActive = season.IsAnimeInfoActive;
                seasonRating.IsSeasonActive = season.IsActive;

                var createdSeasonRating = await seasonRatingWriteRepo.CreateSeasonRating(seasonRating);
                var responseModel = createdSeasonRating.ToCreationResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(SeasonRatingCreationResponseModel)}.{nameof(SeasonRatingCreationResponseModel.Id)}: [{responseModel.Id}]");
                return responseModel;
            }
            catch (EmptyObjectException<SeasonRatingCreationRequestModel> ex)
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
            catch (AlreadyExistingObjectException<SeasonRating> alreadyExistingEx)
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
