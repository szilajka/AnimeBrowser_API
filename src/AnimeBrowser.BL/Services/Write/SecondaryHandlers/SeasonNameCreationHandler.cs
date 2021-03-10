using AnimeBrowser.BL.Helpers;
using AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces;
using AnimeBrowser.BL.Validators.SecondaryValidators;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
using AnimeBrowser.Common.Models.ResponseModels.SecondaryModels;
using AnimeBrowser.Data.Converters.SecondaryConverters;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.MainInterfaces;
using AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces;
using AnimeBrowser.Data.Interfaces.Write.SecondaryInterfaces;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write.SecondaryHandlers
{
    public class SeasonNameCreationHandler : ISeasonNameCreation
    {
        private readonly ILogger logger = Log.ForContext<SeasonNameCreationHandler>();
        private readonly ISeasonRead seasonReadRepo;
        private readonly ISeasonNameRead seasonNameReadRepo;
        private readonly ISeasonNameWrite seasonNameWriteRepo;

        public SeasonNameCreationHandler(ISeasonRead seasonReadRepo, ISeasonNameRead seasonNameReadRepo, ISeasonNameWrite seasonNameWriteRepo)
        {
            this.seasonReadRepo = seasonReadRepo;
            this.seasonNameReadRepo = seasonNameReadRepo;
            this.seasonNameWriteRepo = seasonNameWriteRepo;
        }

        public async Task<SeasonNameCreationResponseModel> CreateSeasonName(SeasonNameCreationRequestModel seasonNameRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with request model: [{seasonNameRequestModel}].");
                if (seasonNameRequestModel == null)
                {
                    var errorModel = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(), description: $"The {nameof(SeasonNameCreationRequestModel)} object is empty!",
                        source: nameof(seasonNameRequestModel), title: ErrorCodes.EmptyObject.GetDescription());
                    throw new EmptyObjectException<SeasonNameCreationRequestModel>(errorModel, $"The given {nameof(SeasonNameCreationRequestModel)} object is empty!");
                }
                var season = await seasonReadRepo.GetSeasonById(seasonNameRequestModel.SeasonId);
                if (season == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyProperty.GetIntValueAsString(),
                        description: $"No {nameof(Season)} object was found with the {nameof(SeasonName)}'s {nameof(SeasonNameCreationRequestModel.SeasonId)} [{seasonNameRequestModel.SeasonId}]!",
                        source: nameof(SeasonNameCreationRequestModel.SeasonId), title: ErrorCodes.EmptyProperty.GetDescription()
                    );
                    var notExistingSeasonEx = new NotFoundObjectException<Season>(error, $"There is no {nameof(Season)} object that was given in {nameof(SeasonNameCreationRequestModel.SeasonId)} property.");
                    throw notExistingSeasonEx;
                }

                var validator = new SeasonNameCreationValidator();
                var validationResult = await validator.ValidateAsync(seasonNameRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(SeasonNameCreationRequestModel)}].{Environment.NewLine}Validation errors:[{string.Join(", ", errorList)}].");
                }

                seasonNameRequestModel.Title = seasonNameRequestModel.Title.Trim();
                var isExistsWithSameTitle = seasonNameReadRepo.IsExistsWithSameTitle(seasonNameRequestModel.Title, seasonNameRequestModel.SeasonId);
                if (isExistsWithSameTitle || season.Title.Equals(seasonNameRequestModel.Title, StringComparison.OrdinalIgnoreCase))
                {
                    var error = new ErrorModel(code: ErrorCodes.NotUniqueProperty.GetIntValueAsString(), description: $"Another {nameof(SeasonName)} can be found in the same {nameof(Season)} [{seasonNameRequestModel.SeasonId}] " +
                       $"and the same {nameof(SeasonNameCreationRequestModel.Title)} [{seasonNameRequestModel.Title}].",
                       source: nameof(SeasonNameCreationRequestModel.Title), title: ErrorCodes.NotUniqueProperty.GetDescription());
                    var alreadyExistingEx = new AlreadyExistingObjectException<SeasonName>(error, $"There is already a {nameof(SeasonName)} with the same {nameof(Season)} and the same {nameof(SeasonName.Title)} value.");
                    throw alreadyExistingEx;
                }

                var seasonName = await seasonNameWriteRepo.CreateSeasonName(seasonNameRequestModel.ToSeasonName());
                var responseModel = seasonName.ToCreationResponseModel();
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(SeasonNameCreationResponseModel)}.{nameof(SeasonNameCreationResponseModel.Id)}: [{responseModel.Id}]");
                return responseModel;
            }
            catch (EmptyObjectException<SeasonNameCreationRequestModel> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (NotFoundObjectException<Season> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                throw;
            }
            catch (AlreadyExistingObjectException<SeasonName> alreadyExistingEx)
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
