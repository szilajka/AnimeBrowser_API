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
    public class GenreEditingHandler : IGenreEditing
    {
        private readonly ILogger logger = Log.ForContext<GenreEditingHandler>();
        private readonly IGenreRead genreReadRepo;
        private readonly IGenreWrite genreWriteRepo;

        public GenreEditingHandler(IGenreRead genreRead, IGenreWrite genreWrite)
        {
            this.genreReadRepo = genreRead;
            this.genreWriteRepo = genreWrite;
        }

        public async Task<GenreEditingResponseModel> EditGenre(long id, GenreEditingRequestModel requestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with requestModel: [{requestModel}].");

                if (id != requestModel?.Id)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                       description: $"The parameter [{nameof(id)}] and [{nameof(requestModel)}.{nameof(GenreEditingRequestModel.Id)}] properties should have the same value, but they are different!",
                       source: nameof(id), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var argEx = new MismatchingIdException(error, "The given id and the model's id are not matching!");
                    logger.Warning(argEx, $"Id mismatch in property and parameter.");
                    throw argEx;
                }

                var validator = new GenreEditingValidator();
                var validationResult = await validator.ValidateAsync(requestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(GenreEditingRequestModel)}].");
                }

                var genre = await genreReadRepo.GetGenreById(id);
                if (genre == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                        description: $"No {nameof(Genre)} object was found with the given id [{id}]!",
                        source: nameof(GenreEditingRequestModel.Id), title: ErrorCodes.EmptyObject.GetDescription()
                    );
                    var notExistingGenreEx = new NotFoundObjectException<GenreEditingRequestModel>(error, $"There is no {nameof(Genre)} with given id: [{id}].");
                    logger.Warning(notExistingGenreEx, notExistingGenreEx.Message);
                    throw notExistingGenreEx;
                }

                var rGenre = requestModel.ToGenre();

                genre.GenreName = rGenre.GenreName;
                genre.Description = rGenre.Description;

                genre = await genreWriteRepo.UpdateGenre(genre);
                GenreEditingResponseModel responseModel = genre.ToEditingResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. result: [{responseModel}].");

                return responseModel;
            }
            catch (ValidationException valEx)
            {
                logger.Warning($"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<GenreEditingRequestModel> ex)
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
