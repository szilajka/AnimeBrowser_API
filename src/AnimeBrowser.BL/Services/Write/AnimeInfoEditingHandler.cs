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
    public class AnimeInfoEditingHandler : IAnimeInfoEditing
    {
        private readonly ILogger logger = Log.ForContext<AnimeInfoEditingHandler>();
        private readonly IAnimeInfoRead animeInfoReadRepo;
        private readonly IAnimeInfoWrite animeInfoWriteRepo;

        public AnimeInfoEditingHandler(IAnimeInfoRead animeInfoReadRepo, IAnimeInfoWrite animeInfoWriteRepo)
        {
            this.animeInfoReadRepo = animeInfoReadRepo;
            this.animeInfoWriteRepo = animeInfoWriteRepo;
        }

        public async Task<AnimeInfoEditingResponseModel> EditAnimeInfo(long id, AnimeInfoEditingRequestModel animeInfoRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(id)}: [{id}], {nameof(animeInfoRequestModel)}: [{animeInfoRequestModel}].");

                if (id != animeInfoRequestModel?.Id)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                        description: $"The parameter [{nameof(id)}] and [{nameof(animeInfoRequestModel)}.{nameof(AnimeInfoEditingRequestModel.Id)}] properties should have the same value, but they are different!",
                        source: nameof(id), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var mismatchingIdEx = new MismatchingIdException(error, "The given id and the model's id are not matching!");
                    throw mismatchingIdEx;
                }

                var validator = new AnimeInfoEditingValidator();
                var validationResult = await validator.ValidateAsync(animeInfoRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(AnimeInfoEditingRequestModel)}].{Environment.NewLine}Validation errors:[{string.Join(", ", errorList)}].");
                }

                var animeInfo = await animeInfoReadRepo.GetAnimeInfoById(id);
                if (animeInfo == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                       description: $"No {nameof(AnimeInfo)} object was found with the given id [{id}]!",
                       source: nameof(AnimeInfoEditingRequestModel.Id), title: ErrorCodes.EmptyObject.GetDescription()
                   );
                    var notExistingAnimeInfoEx = new NotFoundObjectException<AnimeInfo>(error, $"There is no [{nameof(AnimeInfo)}] with given id: [{id}].");
                    logger.Warning(notExistingAnimeInfoEx, notExistingAnimeInfoEx.Message);
                    throw notExistingAnimeInfoEx;
                }

                var rAnimeInfo = animeInfoRequestModel.ToAnimeInfo();

                animeInfo.Title = rAnimeInfo.Title;
                animeInfo.Description = rAnimeInfo.Description;
                animeInfo.IsNsfw = rAnimeInfo.IsNsfw;

                animeInfo = await animeInfoWriteRepo.UpdateAnimeInfo(animeInfo);
                AnimeInfoEditingResponseModel responseModel = animeInfo.ToEditingResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(AnimeInfoEditingResponseModel)}.{nameof(AnimeInfoEditingResponseModel.Id)}: [{responseModel.Id}].");

                return responseModel;
            }
            catch (MismatchingIdException mismatchIdEx)
            {
                logger.Warning(mismatchIdEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{mismatchIdEx.Message}].");
                throw;
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<AnimeInfo> ex)
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
