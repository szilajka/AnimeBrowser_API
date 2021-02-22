using AnimeBrowser.BL.Helpers;
using AnimeBrowser.BL.Interfaces.DateTimeProviders;
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
    public class SeasonCreationHandler : ISeasonCreation
    {
        private readonly ILogger logger = Log.ForContext<SeasonCreationHandler>();
        private readonly IDateTime dateTimeProvider;
        private readonly ISeasonWrite seasonWriteRepo;
        private readonly IAnimeInfoRead animeInfoReadRepo;

        public SeasonCreationHandler(IDateTime dateTimeProvider, ISeasonWrite seasonWrite, IAnimeInfoRead animeInfoRead)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.seasonWriteRepo = seasonWrite;
            this.animeInfoReadRepo = animeInfoRead;
        }

        public async Task<SeasonCreationResponseModel> CreateSeason(SeasonCreationRequestModel seasonRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. requestModel: [{seasonRequestModel}].");
                if (seasonRequestModel == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(), description: $"The {nameof(SeasonCreationRequestModel)} (variabble: {nameof(seasonRequestModel)}) object is empty!",
                        source: nameof(seasonRequestModel), title: ErrorCodes.EmptyObject.GetDescription());
                    throw new EmptyObjectException<SeasonCreationRequestModel>(error, $"The given [{nameof(Season)}] object is empty!");
                }

                seasonRequestModel.Title = seasonRequestModel.Title.Trim();
                seasonRequestModel.Description = seasonRequestModel.Description.Trim();

                var seasonValidator = new SeasonCreationValidator(dateTimeProvider);
                var validationResult = await seasonValidator.ValidateAsync(seasonRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(SeasonCreationRequestModel)}].");
                }
                var animeInfo = await animeInfoReadRepo.GetAnimeInfoById(seasonRequestModel.AnimeInfoId);
                if (animeInfo == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyProperty.GetIntValueAsString(),
                        description: $"The {nameof(AnimeInfo)} object is empty that is linked with the current {nameof(Season)} [{nameof(SeasonCreationRequestModel.AnimeInfoId)}: {seasonRequestModel?.AnimeInfoId}]!",
                        source: nameof(SeasonCreationRequestModel.AnimeInfoId), title: ErrorCodes.EmptyProperty.GetDescription()
                    );
                    var notExistingAnimeInfoEx = new NotFoundObjectException<AnimeInfo>(error, $"There is no {nameof(AnimeInfo)} object that was given in {nameof(SeasonCreationRequestModel.AnimeInfoId)} property.");
                    logger.Warning(notExistingAnimeInfoEx, notExistingAnimeInfoEx.Message);
                    throw notExistingAnimeInfoEx;
                }
                var createdSeason = await seasonWriteRepo.CreateSeason(seasonRequestModel.ToSeason());

                var responseModel = createdSeason.ToCreationResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. responseModel: [{responseModel}].");

                return responseModel;
            }
            catch (ValidationException valEx)
            {
                logger.Warning($"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                throw;
            }
            catch (EmptyObjectException<SeasonCreationRequestModel> ex)
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
