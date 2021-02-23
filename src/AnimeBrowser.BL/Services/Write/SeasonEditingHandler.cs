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
    public class SeasonEditingHandler : ISeasonEditing
    {
        private readonly ILogger logger = Log.ForContext<SeasonEditingHandler>();
        private readonly IDateTime dateTimeProvider;
        private readonly ISeasonRead seasonReadRepo;
        private readonly ISeasonWrite seasonWriteRepo;

        public SeasonEditingHandler(IDateTime dateTimeProvider, ISeasonRead seasonReadRepo, ISeasonWrite seasonWriteRepo)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.seasonReadRepo = seasonReadRepo;
            this.seasonWriteRepo = seasonWriteRepo;
        }

        public async Task<SeasonEditingResponseModel> EditSeason(long id, SeasonEditingRequestModel seasonRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with requestModel: [{seasonRequestModel}].");

                if (id != seasonRequestModel?.Id)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                       description: $"The parameter [{nameof(id)}] and [{nameof(seasonRequestModel)}.{nameof(SeasonEditingRequestModel.Id)}] properties should have the same value, but they are different!",
                       source: nameof(id), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var argEx = new MismatchingIdException(error, "The given id and the model's id are not matching!");
                    logger.Warning(argEx, $"Id mismatch in property and parameter.");
                    throw argEx;
                }

                var validator = new SeasonEditingValidator(dateTimeProvider);
                var validationResult = await validator.ValidateAsync(seasonRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(SeasonEditingRequestModel)}].");
                }

                var season = await seasonReadRepo.GetSeasonById(id);
                if (season == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                        description: $"No {nameof(Season)} object was found with the given id [{id}]!",
                        source: nameof(SeasonEditingRequestModel.Id), title: ErrorCodes.EmptyObject.GetDescription()
                    );
                    var notExistingSeasonEx = new NotFoundObjectException<SeasonEditingRequestModel>(error, $"There is no {nameof(Season)} with given id: [{id}].");
                    logger.Warning(notExistingSeasonEx, notExistingSeasonEx.Message);
                    throw notExistingSeasonEx;
                }

                var rSeason = seasonRequestModel.ToSeason();
                season.UpdateSeasonWithOtherSeason(rSeason);

                season = await seasonWriteRepo.UpdateSeason(season);
                SeasonEditingResponseModel responseModel = season.ToEditingResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. result: [{responseModel}].");

                return responseModel;
            }
            catch (ValidationException valEx)
            {
                logger.Warning($"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<SeasonEditingRequestModel> ex)
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
