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

        public async Task<AnimeInfoEditingResponseModel> EditAnimeInfo(long id, AnimeInfoEditingRequestModel requestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with requestModel: [{requestModel}].");

                if (id != requestModel?.Id)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                        description: $"The parameter [{nameof(id)}] and [{nameof(requestModel)}.{nameof(AnimeInfoEditingRequestModel.Id)}] properties should have the same value, but they are different!",
                        source: nameof(id), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var argEx = new MismatchingIdException(error, "The given id and the model's id are not matching!");
                    logger.Warning(argEx, $"Id mismatch in property and parameter.");
                    throw argEx;
                }

                var validator = new AnimeInfoEditingValidator();
                var validationResult = await validator.ValidateAsync(requestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(AnimeInfoEditingRequestModel)}].");
                }

                var animeInfo = await animeInfoReadRepo.GetAnimeInfoById(id);
                if (animeInfo == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                       description: $"No {nameof(AnimeInfo)} object was found with the given id [{animeInfo?.Id}]!",
                       source: nameof(AnimeInfoEditingRequestModel.Id), title: ErrorCodes.EmptyObject.GetDescription()
                   );
                    var notExistingAnimeInfoEx = new NotFoundObjectException<AnimeInfoEditingRequestModel>(error, $"There is no [{nameof(AnimeInfo)}] with given id: [{id}].");
                    logger.Warning(notExistingAnimeInfoEx, notExistingAnimeInfoEx.Message);
                    throw notExistingAnimeInfoEx;
                }

                var rAnimeInfo = requestModel.ToAnimeInfo();

                animeInfo.Title = rAnimeInfo.Title;
                animeInfo.Description = rAnimeInfo.Description;
                animeInfo.IsNsfw = rAnimeInfo.IsNsfw;

                animeInfo = await animeInfoWriteRepo.UpdateAnimeInfo(animeInfo);
                AnimeInfoEditingResponseModel responseModel = animeInfo.ToEditingResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. result: [{responseModel}].");

                return responseModel;
            }
            catch (ValidationException valEx)
            {
                logger.Warning($"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<AnimeInfoEditingRequestModel> ex)
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
