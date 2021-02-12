using AnimeBrowser.BL.Helpers;
using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Validators;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Common.Models.ResponseModels;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write
{
    public class AnimeInfoCreationHandler : IAnimeInfoCreation
    {
        public async Task<AnimeInfoCreationResponseModel> CreateAnimeInfo(AnimeInfoCreationRequestModel animeInfo)
        {
            try
            {
                if (animeInfo == null)
                {
                    throw new EmptyObjectException<AnimeInfoCreationRequestModel>("The given anime info object is empty!");
                }
                animeInfo.Title = animeInfo?.Title?.Trim();
                animeInfo.Description = animeInfo?.Description?.Trim();
                var validator = new AnimeInfoCreationValidator();
                var validationResult = await validator.ValidateAsync(animeInfo);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, "Validation error in AnimeInfoCreationRequestModel.");
                }
                //TODO: save to db, then return the saved item instead of this
                return new AnimeInfoCreationResponseModel();
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
