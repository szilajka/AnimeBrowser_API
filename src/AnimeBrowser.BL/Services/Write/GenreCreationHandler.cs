using AnimeBrowser.BL.Helpers;
using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Validators;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Common.Models.ResponseModels;
using AnimeBrowser.Data.Converters;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Write;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write
{
    public class GenreCreationHandler : IGenreCreation
    {
        private readonly ILogger logger = Log.ForContext<GenreCreationHandler>();
        private readonly IGenreWrite genreWriteRepo;

        public GenreCreationHandler(IGenreWrite genreWrite)
        {
            this.genreWriteRepo = genreWrite;
        }

        public async Task<GenreCreationResponseModel> CreateGenre(GenreCreationRequestModel requestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. requestModel: [{requestModel}].");
                if (requestModel == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(), description: $"The {nameof(GenreCreationRequestModel)} (variabble: {nameof(requestModel)}) object is empty!",
                        source: nameof(requestModel), title: ErrorCodes.EmptyObject.GetDescription());
                    throw new EmptyObjectException<GenreCreationRequestModel>(error, $"The given [{nameof(Genre)}] object is empty!");
                }

                if (!string.IsNullOrWhiteSpace(requestModel.GenreName))
                {
                    requestModel.GenreName = requestModel.GenreName.Trim();
                }
                if (!string.IsNullOrWhiteSpace(requestModel.Description))
                {
                    requestModel.Description = requestModel.Description.Trim();
                }

                var genreValidator = new GenreCreationValidator();
                var validationResult = await genreValidator.ValidateAsync(requestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(GenreCreationRequestModel)}].");
                }
                var createdGenre = await genreWriteRepo.CreateGenre(requestModel.ToGenre());

                var responseModel = createdGenre.ToCreationResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. responseModel: [{responseModel}].");

                return responseModel;
            }
            catch (ValidationException valEx)
            {
                logger.Warning($"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                throw;
            }
            catch (EmptyObjectException<GenreCreationRequestModel> ex)
            {
                logger.Warning($"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
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
