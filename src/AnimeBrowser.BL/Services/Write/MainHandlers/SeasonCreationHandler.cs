using AnimeBrowser.BL.Helpers;
using AnimeBrowser.BL.Interfaces.DateTimeProviders;
using AnimeBrowser.BL.Interfaces.Write.MainInterfaces;
using AnimeBrowser.BL.Validators.MainValidators;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels.MainModels;
using AnimeBrowser.Common.Models.ResponseModels.MainModels;
using AnimeBrowser.Data.Converters.MainConverters;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.MainInterfaces;
using AnimeBrowser.Data.Interfaces.Write.MainInterfaces;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write.MainHandlers
{
    public class SeasonCreationHandler : ISeasonCreation
    {
        private readonly ILogger logger = Log.ForContext<SeasonCreationHandler>();
        private readonly IDateTime dateTimeProvider;
        private readonly ISeasonWrite seasonWriteRepo;
        private readonly ISeasonRead seasonReadRepo;
        private readonly IAnimeInfoRead animeInfoReadRepo;

        public SeasonCreationHandler(IDateTime dateTimeProvider, ISeasonWrite seasonWrite, ISeasonRead seasonReadRepo, IAnimeInfoRead animeInfoReadRepo)
        {
            this.dateTimeProvider = dateTimeProvider;
            seasonWriteRepo = seasonWrite;
            this.seasonReadRepo = seasonReadRepo;
            this.animeInfoReadRepo = animeInfoReadRepo;
        }

        public async Task<SeasonCreationResponseModel> CreateSeason(SeasonCreationRequestModel seasonRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonRequestModel)}: [{seasonRequestModel}].");
                if (seasonRequestModel == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(), description: $"The {nameof(SeasonCreationRequestModel)} object is empty!",
                        source: nameof(seasonRequestModel), title: ErrorCodes.EmptyObject.GetDescription());
                    throw new EmptyObjectException<SeasonCreationRequestModel>(error, $"The given [{nameof(Season)}] object is empty!");
                }

                var seasonValidator = new SeasonCreationValidator(dateTimeProvider);
                var validationResult = await seasonValidator.ValidateAsync(seasonRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    var valEx = new ValidationException(errorList, $"Validation error in [{nameof(SeasonCreationRequestModel)}].{Environment.NewLine}Validation errors:[{string.Join(", ", errorList)}].");
                    throw valEx;
                }

                var isExistingSeasonWithSameSeasonNumber = seasonReadRepo.IsExistsSeasonWithSeasonNumber(animeInfoId: seasonRequestModel.AnimeInfoId, seasonNumber: seasonRequestModel.SeasonNumber);
                if (isExistingSeasonWithSameSeasonNumber)
                {
                    var error = new ErrorModel(code: ErrorCodes.NotUniqueProperty.GetIntValueAsString(), description: $"Another {nameof(Season)} can be found in the same {nameof(AnimeInfo)} [{seasonRequestModel.AnimeInfoId}] " +
                        $"with the same {nameof(SeasonCreationRequestModel.SeasonNumber)} [{seasonRequestModel.SeasonNumber}].",
                        source: nameof(SeasonCreationRequestModel.SeasonNumber), title: ErrorCodes.NotUniqueProperty.GetDescription());
                    var alreadyExistingEx = new AlreadyExistingObjectException<Season>(error, $"There is already a {nameof(Season)} in the same {nameof(AnimeInfo)} with the same {nameof(Season.SeasonNumber)} value.");
                    throw alreadyExistingEx;
                }

                var animeInfo = await animeInfoReadRepo.GetAnimeInfoById(seasonRequestModel.AnimeInfoId);
                if (animeInfo == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyProperty.GetIntValueAsString(),
                        description: $"No {nameof(AnimeInfo)} object was found with the {nameof(Season)}'s {nameof(SeasonCreationRequestModel.AnimeInfoId)} [{seasonRequestModel?.AnimeInfoId}]!",
                        source: nameof(SeasonCreationRequestModel.AnimeInfoId), title: ErrorCodes.EmptyProperty.GetDescription()
                    );
                    var notExistingAnimeInfoEx = new NotFoundObjectException<AnimeInfo>(error, $"There is no {nameof(AnimeInfo)} object that was given in {nameof(SeasonCreationRequestModel.AnimeInfoId)} property.");
                    throw notExistingAnimeInfoEx;
                }
                var createdSeason = await seasonWriteRepo.CreateSeason(seasonRequestModel.ToSeason());

                var responseModel = createdSeason.ToCreationResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(SeasonCreationResponseModel)}.{nameof(SeasonCreationResponseModel.Id)}: [{responseModel.Id}].");

                return responseModel;
            }
            catch (EmptyObjectException<SeasonCreationRequestModel> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                throw;
            }
            catch (AlreadyExistingObjectException<Season> alreadyExistingEx)
            {
                logger.Warning(alreadyExistingEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{alreadyExistingEx.Message}].");
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
