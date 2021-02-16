using AnimeBrowser.BL.Helpers;
using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Validators;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Common.Models.ResponseModels;
using AnimeBrowser.Data.Converters;
using AnimeBrowser.Data.Interfaces.Read;
using AnimeBrowser.Data.Interfaces.Write;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write
{
    public class AnimeInfoEditorHandler : IAnimeInfoEditor
    {
        private readonly ILogger logger = Log.ForContext<AnimeInfoEditorHandler>();
        private readonly IAnimeInfoRead animeInfoReadRepo;
        private readonly IAnimeInfoWrite animeInfoWriteRepo;

        public AnimeInfoEditorHandler(IAnimeInfoRead animeInfoReadRepo, IAnimeInfoWrite animeInfoWriteRepo)
        {
            this.animeInfoReadRepo = animeInfoReadRepo;
            this.animeInfoWriteRepo = animeInfoWriteRepo;
        }

        public async Task<AnimeInfoEditingResponseModel> EditAnimeInfo(long id, AnimeInfoEditingRequestModel requestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with requestModel: [{requestModel}].");

                if (id != requestModel.Id)
                {
                    var argEx = new ArgumentException("The given id and the model's id are not matching!", nameof(id));
                    logger.Warning(argEx, $"Id mismatch in property and parameter.");
                    throw argEx;
                }

                var animeInfo = await animeInfoReadRepo.GetAnimeInfoById(id);
                if (animeInfo == null)
                {
                    var notExistingAnimeInfoEx = new NotFoundObjectException<AnimeInfoCreationRequestModel>($"There is no AnimeInfo with given id: [{id}].");
                    logger.Warning(notExistingAnimeInfoEx, notExistingAnimeInfoEx.Message);
                    throw notExistingAnimeInfoEx;
                }

                var validator = new AnimeInfoEditingValidator();
                var validationResult = await validator.ValidateAsync(requestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, "Validation error in AnimeInfoCreationRequestModel.");
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
            catch (NotFoundObjectException<AnimeInfoCreationRequestModel> ex)
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
