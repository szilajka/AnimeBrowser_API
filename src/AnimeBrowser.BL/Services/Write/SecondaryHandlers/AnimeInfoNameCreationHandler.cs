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
using AnimeBrowser.Data.Interfaces.Read.MainInterfaces;
using AnimeBrowser.Data.Interfaces.Write.SecondaryInterfaces;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write.SecondaryHandlers
{
    public class AnimeInfoNameCreationHandler : IAnimeInfoNameCreation
    {
        private readonly ILogger logger = Log.ForContext<AnimeInfoNameCreationHandler>();
        private readonly IAnimeInfoNameWrite animeInfoNameWriteRepo;
        private readonly IAnimeInfoRead animeInfoReadRepo;

        public AnimeInfoNameCreationHandler(IAnimeInfoNameWrite animeInfoNameWriteRepo, IAnimeInfoRead animeInfoReadRepo)
        {
            this.animeInfoNameWriteRepo = animeInfoNameWriteRepo;
            this.animeInfoReadRepo = animeInfoReadRepo;
        }

        public async Task<AnimeInfoNameCreationResponseModel> CreateAnimeInfoName(AnimeInfoNameCreationRequestModel animeInfoNameRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with request model: [{animeInfoNameRequestModel}].");
                if (animeInfoNameRequestModel == null)
                {
                    var errorModel = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(), description: $"The {nameof(AnimeInfoNameCreationRequestModel)} object is empty!",
                        source: nameof(animeInfoNameRequestModel), title: ErrorCodes.EmptyObject.GetDescription());
                    throw new EmptyObjectException<AnimeInfoNameCreationRequestModel>(errorModel, $"The given {nameof(AnimeInfoNameCreationRequestModel)} object is empty!");
                }
                var animeInfo = await animeInfoReadRepo.GetAnimeInfoById(animeInfoNameRequestModel.AnimeInfoId);
                if (animeInfo == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyProperty.GetIntValueAsString(),
                        description: $"No {nameof(AnimeInfo)} object was found with the {nameof(AnimeInfoName)}'s {nameof(AnimeInfoNameCreationRequestModel.AnimeInfoId)} [{animeInfoNameRequestModel.AnimeInfoId}]!",
                        source: nameof(AnimeInfoNameCreationRequestModel.AnimeInfoId), title: ErrorCodes.EmptyProperty.GetDescription()
                    );
                    var notExistingAnimeInfoEx = new NotFoundObjectException<AnimeInfo>(error, $"There is no {nameof(AnimeInfo)} object that was given in {nameof(AnimeInfoNameCreationRequestModel.AnimeInfoId)} property.");
                    throw notExistingAnimeInfoEx;
                }

                var validator = new AnimeInfoNameCreationValidator();
                var validationResult = await validator.ValidateAsync(animeInfoNameRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(AnimeInfoNameCreationRequestModel)}].{Environment.NewLine}Validation errors:[{string.Join(", ", errorList)}].");
                }

                var animeInfoName = await animeInfoNameWriteRepo.CreateAnimeInfoName(animeInfoNameRequestModel.ToAnimeInfoName());
                var responseModel = animeInfoName.ToCreationResponseModel();
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(AnimeInfoNameCreationResponseModel)}.{nameof(AnimeInfoNameCreationResponseModel.Id)}: [{responseModel.Id}]");
                return responseModel;
            }
            catch (EmptyObjectException<AnimeInfoNameCreationRequestModel> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (NotFoundObjectException<AnimeInfo> ex)
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
                logger.Error(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
        }
    }
}
