using AnimeBrowser.BL.Helpers;
using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Validators;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Common.Models.ResponseModels;
using AnimeBrowser.Data.Converters;
using AnimeBrowser.Data.Interfaces.Write;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    throw new EmptyObjectException<GenreCreationRequestModel>("The given genre object is empty!");
                }

                requestModel.GenreName = requestModel.GenreName?.Trim();
                requestModel.Description = requestModel.Description?.Trim();

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
