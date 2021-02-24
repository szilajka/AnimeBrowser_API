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

        public async Task<GenreEditingResponseModel> EditGenre(long id, GenreEditingRequestModel genreRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with {nameof(genreRequestModel)}: [{genreRequestModel}].");

                if (id != genreRequestModel?.Id)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                       description: $"The parameter [{nameof(id)}] and [{nameof(genreRequestModel)}.{nameof(GenreEditingRequestModel.Id)}] properties should have the same value, but they are different!",
                       source: nameof(id), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var mismatchEx = new MismatchingIdException(error, "The given id and the model's id are not matching!");
                    throw mismatchEx;
                }

                var validator = new GenreEditingValidator();
                var validationResult = await validator.ValidateAsync(genreRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(GenreEditingRequestModel)}].{Environment.NewLine}Validation errors:[{string.Join(", ", errorList)}].");
                }

                var genre = await genreReadRepo.GetGenreById(id);
                if (genre == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                        description: $"No {nameof(Genre)} object was found with the given id [{id}]!",
                        source: nameof(GenreEditingRequestModel.Id), title: ErrorCodes.EmptyObject.GetDescription()
                    );
                    var notExistingGenreEx = new NotFoundObjectException<Genre>(error, $"There is no {nameof(Genre)} with given id: [{id}].");
                    logger.Warning(notExistingGenreEx, notExistingGenreEx.Message);
                    throw notExistingGenreEx;
                }

                var rGenre = genreRequestModel.ToGenre();

                genre.GenreName = rGenre.GenreName;
                genre.Description = rGenre.Description;

                genre = await genreWriteRepo.UpdateGenre(genre);
                GenreEditingResponseModel responseModel = genre.ToEditingResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(GenreEditingResponseModel)}.{nameof(GenreEditingResponseModel.Id)}: [{responseModel.Id}].");

                return responseModel;
            }
            catch (MismatchingIdException mismatchEx)
            {
                logger.Warning(mismatchEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{mismatchEx.Message}].");
                throw;
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<Genre> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
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
