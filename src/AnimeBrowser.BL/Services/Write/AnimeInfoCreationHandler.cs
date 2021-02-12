using AnimeBrowser.BL.Helpers;
using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Validators;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
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
    public class AnimeInfoCreationHandler : IAnimeInfoCreation
    {
        private readonly IAnimeInfoWrite animeInfoWriteRepo;

        public AnimeInfoCreationHandler(IAnimeInfoWrite animeInfoWriteRepo)
        {
            this.animeInfoWriteRepo = animeInfoWriteRepo;
        }

        public async Task<AnimeInfoCreationResponseModel> CreateAnimeInfo(AnimeInfoCreationRequestModel animeInfoRequestModel)
        {
            try
            {
                Log.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with request model: [{animeInfoRequestModel}].");
                if (animeInfoRequestModel == null)
                {
                    throw new EmptyObjectException<AnimeInfoCreationRequestModel>("The given anime info object is empty!");
                }

                animeInfoRequestModel.Title = animeInfoRequestModel?.Title?.Trim();
                animeInfoRequestModel.Description = animeInfoRequestModel?.Description?.Trim();

                var validator = new AnimeInfoCreationValidator();
                var validationResult = await validator.ValidateAsync(animeInfoRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, "Validation error in AnimeInfoCreationRequestModel.");
                }

                var animeInfo = await animeInfoWriteRepo.CreateAnimeInfo(animeInfoRequestModel.ToAnimeInfo());
                var responseModel = animeInfo.ToCreationResponseModel();
                Log.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Created model id: [{responseModel.Id}]");
                return responseModel;
            }
            catch (ValidationException valEx)
            {
                Log.Warning($"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                throw;
            }
            catch (EmptyObjectException<AnimeInfoCreationRequestModel> ex)
            {
                Log.Warning($"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
        }
    }
}
