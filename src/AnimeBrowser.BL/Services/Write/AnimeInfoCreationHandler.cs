using AnimeBrowser.BL.Helpers;
using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Validators;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Common.Models.ResponseModels;
using AnimeBrowser.Data.Converters;
using AnimeBrowser.Data.Interfaces.Write;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write
{
    public class AnimeInfoCreationHandler : IAnimeInfoCreation
    {
        private readonly IAnimeInfoWrite animeInfoWriteRepo;
        private readonly ILogger logger = Log.ForContext<AnimeInfoCreationHandler>();

        public AnimeInfoCreationHandler(IAnimeInfoWrite animeInfoWriteRepo)
        {
            this.animeInfoWriteRepo = animeInfoWriteRepo;
        }

        public async Task<AnimeInfoCreationResponseModel> CreateAnimeInfo(AnimeInfoCreationRequestModel animeInfoRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with request model: [{animeInfoRequestModel}].");
                if (animeInfoRequestModel == null)
                {
                    var errorModel = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(), description: $"The {nameof(AnimeInfoCreationRequestModel)} object is empty!",
                        source: nameof(animeInfoRequestModel), title: ErrorCodes.EmptyObject.GetDescription());
                    throw new EmptyObjectException<AnimeInfoCreationRequestModel>(errorModel, $"The given {nameof(AnimeInfoCreationRequestModel)} object is empty!");
                }

                var validator = new AnimeInfoCreationValidator();
                var validationResult = await validator.ValidateAsync(animeInfoRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(AnimeInfoCreationRequestModel)}].{Environment.NewLine}Validation errors:[{string.Join(", ", errorList)}].");
                }

                var animeInfo = await animeInfoWriteRepo.CreateAnimeInfo(animeInfoRequestModel.ToAnimeInfo());
                var responseModel = animeInfo.ToCreationResponseModel();
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(AnimeInfoCreationResponseModel)}.{nameof(AnimeInfoCreationResponseModel.Id)}: [{responseModel.Id}]");
                return responseModel;
            }
            catch (EmptyObjectException<AnimeInfoCreationRequestModel> ex)
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
