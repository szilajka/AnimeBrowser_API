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
using AnimeBrowser.Data.Interfaces.Read;
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
        private readonly IGenreRead genreReadRepo;

        public GenreCreationHandler(IGenreWrite genreWrite, IGenreRead genreReadRepo)
        {
            this.genreWriteRepo = genreWrite;
            this.genreReadRepo = genreReadRepo;
        }

        public async Task<GenreCreationResponseModel> CreateGenre(GenreCreationRequestModel genreRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(genreRequestModel)}: [{genreRequestModel}].");
                if (genreRequestModel == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(), description: $"The {nameof(GenreCreationRequestModel)} object is empty!",
                        source: nameof(genreRequestModel), title: ErrorCodes.EmptyObject.GetDescription());
                    throw new EmptyObjectException<GenreCreationRequestModel>(error, $"The given [{nameof(GenreCreationRequestModel)}] object is empty!");
                }

                var genreValidator = new GenreCreationValidator();
                var validationResult = await genreValidator.ValidateAsync(genreRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(GenreCreationRequestModel)}].{Environment.NewLine}Validation errors:[{string.Join(", ", errorList)}].");
                }
                var trimmedGenreRequestModel = genreRequestModel.ToGenre();
                var isAlreadyExisting = genreReadRepo.IsExistWithSameName(trimmedGenreRequestModel.GenreName);
                if (isAlreadyExisting)
                {
                    var error = new ErrorModel(code: ErrorCodes.NotUniqueProperty.GetIntValueAsString(), description: $"Another {nameof(Genre)} can be found with the same {nameof(Genre.GenreName)} [{genreRequestModel.GenreName}].",
                        source: nameof(GenreCreationRequestModel.GenreName), title: ErrorCodes.NotUniqueProperty.GetDescription());
                    var alreadyExistingEx = new AlreadyExistingObjectException<Genre>(error, $"There is already an {nameof(Genre)} with the same {nameof(Genre.GenreName)} value.");
                    throw alreadyExistingEx;
                }
                var createdGenre = await genreWriteRepo.CreateGenre(trimmedGenreRequestModel);

                var responseModel = createdGenre.ToCreationResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(GenreCreationResponseModel)}.{nameof(GenreCreationResponseModel.Id)}: [{responseModel.Id}].");

                return responseModel;
            }
            catch (EmptyObjectException<GenreCreationRequestModel> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                throw;
            }
            catch (AlreadyExistingObjectException<Genre> alreadyExistingEx)
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
