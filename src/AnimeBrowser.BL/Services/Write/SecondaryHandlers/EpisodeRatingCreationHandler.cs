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
    public class EpisodeRatingCreationHandler : IEpisodeRatingCreation
    {
        private readonly ILogger logger = Log.ForContext<EpisodeRatingCreationHandler>();
        private readonly IEpisodeRead episodeReadRepo;
        private readonly IUserRead userReadRepo;
        private readonly IEpisodeRatingRead episodeRatingReadRepo;
        private readonly IEpisodeRatingWrite episodeRatingWriteRepo;

        public EpisodeRatingCreationHandler(IEpisodeRead episodeReadRepo, IUserRead userReadRepo, IEpisodeRatingRead episodeRatingReadRepo, IEpisodeRatingWrite episodeRatingWriteRepo)
        {
            this.episodeReadRepo = episodeReadRepo;
            this.userReadRepo = userReadRepo;
            this.episodeRatingReadRepo = episodeRatingReadRepo;
            this.episodeRatingWriteRepo = episodeRatingWriteRepo;
        }

        public async Task<EpisodeRatingCreationResponseModel> CreateEpisodeRating(EpisodeRatingCreationRequestModel episodeRatingRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with request model: [{episodeRatingRequestModel}].");
                if (episodeRatingRequestModel == null)
                {
                    var errorModel = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(), description: $"The {nameof(EpisodeRatingCreationRequestModel)} object is empty!",
                        source: nameof(episodeRatingRequestModel), title: ErrorCodes.EmptyObject.GetDescription());
                    throw new EmptyObjectException<EpisodeRatingCreationRequestModel>(errorModel, $"The given {nameof(EpisodeRatingCreationRequestModel)} object is empty!");
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

                var validator = new EpisodeRatingCreationValidator();
                var validationResult = await validator.ValidateAsync(episodeRatingRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(EpisodeRatingCreationRequestModel)}].{Environment.NewLine}Validation errors: [{string.Join(", ", errorList)}].");
                }

                var existingEpisodeRepo = episodeRatingReadRepo.GetEpisodeRatingByEpisodeAndUserId(episodeRatingRequestModel.EpisodeId, episodeRatingRequestModel.UserId);
                if (existingEpisodeRepo != null)
                {
                    var error = new ErrorModel(code: ErrorCodes.NotUniqueProperty.GetIntValueAsString(), description: $"Another {nameof(EpisodeRating)} can be found with the same {nameof(EpisodeRating.EpisodeId)} [{episodeRatingRequestModel.EpisodeId}] " +
                       $"and the same {nameof(EpisodeRating.UserId)} [{episodeRatingRequestModel.UserId}].",
                       source: $"[{nameof(episodeRatingRequestModel.EpisodeId)}, {nameof(episodeRatingRequestModel.UserId)}]", title: ErrorCodes.NotUniqueProperty.GetDescription());
                    var alreadyExistingEx = new AlreadyExistingObjectException<EpisodeRating>(error, $"There is already a {nameof(EpisodeRating)} with the same {nameof(Episode)} and the same {nameof(User)} value.");
                    throw alreadyExistingEx;
                }

                var episodeRating = episodeRatingRequestModel.ToEpisodeRating();
                var createdEpisodeRating = await episodeRatingWriteRepo.CreateEpisodeRating(episodeRating);
                var responseModel = createdEpisodeRating.ToCreationResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(EpisodeRatingCreationResponseModel)}.{nameof(EpisodeRatingCreationResponseModel.Id)}: [{responseModel.Id}]");
                return responseModel;
            }
            catch (EmptyObjectException<EpisodeRatingCreationRequestModel> ex)
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
            catch (AlreadyExistingObjectException<EpisodeRating> alreadyExistingEx)
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
