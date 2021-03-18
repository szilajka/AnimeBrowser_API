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
    public class SeasonNameEditingHandler : ISeasonNameEditing
    {
        private readonly ILogger logger = Log.ForContext<SeasonNameEditingHandler>();
        private readonly ISeasonRead seasonReadRepo;
        private readonly ISeasonNameRead seasonNameReadRepo;
        private readonly ISeasonNameWrite seasonNameWriteRepo;

        public SeasonNameEditingHandler(ISeasonRead seasonReadRepo, ISeasonNameRead seasonNameReadRepo, ISeasonNameWrite seasonNameWriteRepo)
        {
            this.seasonReadRepo = seasonReadRepo;
            this.seasonNameReadRepo = seasonNameReadRepo;
            this.seasonNameWriteRepo = seasonNameWriteRepo;
        }

        public async Task<SeasonNameEditingResponseModel> EditSeasonName(long id, SeasonNameEditingRequestModel seasonNameRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with {nameof(id)}: [{id}], {nameof(seasonNameRequestModel)}: [{seasonNameRequestModel}].");

                if (id != seasonNameRequestModel?.Id)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                       description: $"The parameter [{nameof(id)}] and [{nameof(seasonNameRequestModel)}.{nameof(seasonNameRequestModel.Id)}] properties should have the same value, but they are different!",
                       source: nameof(id), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var mismatchEx = new MismatchingIdException(error, "The given id and the model's id are not matching!");
                    throw mismatchEx;
                }

                var seasonName = await seasonNameReadRepo.GetSeasonNameById(id);
                if (seasonName == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                        description: $"No {nameof(SeasonName)} object was found with the given id [{id}]!",
                        source: nameof(id), title: ErrorCodes.EmptyObject.GetDescription()
                    );
                    var notExistingSeasonNameEx = new NotFoundObjectException<SeasonName>(error, $"There is no {nameof(SeasonName)} with given id: [{id}].");
                    throw notExistingSeasonNameEx;
                }

                var season = await seasonReadRepo.GetSeasonById(seasonNameRequestModel.SeasonId);
                if (season == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyProperty.GetIntValueAsString(),
                        description: $"The {nameof(Season)} object is empty that is linked with the current {nameof(SeasonName)} [{nameof(seasonNameRequestModel.SeasonId)}: {seasonNameRequestModel.SeasonId}]!",
                        source: nameof(seasonNameRequestModel.SeasonId), title: ErrorCodes.EmptyProperty.GetDescription()
                    );
                    var notExistingSeasonEx = new NotFoundObjectException<Season>(error, $"There is no {nameof(Season)} object that was given in {nameof(seasonNameRequestModel.SeasonId)} property.");
                    throw notExistingSeasonEx;
                }

                var validator = new SeasonNameEditingValidator();
                var validationResult = await validator.ValidateAsync(seasonNameRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(SeasonNameEditingRequestModel)}].{Environment.NewLine}Validation errors: [{string.Join(", ", errorList)}].");
                }

                seasonNameRequestModel.Title = seasonNameRequestModel.Title.Trim();
                var isExistingWithSameTitle = seasonNameReadRepo.IsExistsWithSameTitle(seasonNameRequestModel.Id, seasonNameRequestModel.Title, seasonNameRequestModel.SeasonId);
                if (isExistingWithSameTitle || season.Title.Equals(seasonNameRequestModel.Title, StringComparison.OrdinalIgnoreCase))
                {
                    var error = new ErrorModel(code: ErrorCodes.NotUniqueProperty.GetIntValueAsString(), description: $"Another {nameof(SeasonName)} can be found with the same {nameof(Season)} [{seasonNameRequestModel.SeasonId}] " +
                       $"and the same {nameof(seasonNameRequestModel.Title)} [{seasonNameRequestModel.Title}].",
                       source: nameof(seasonNameRequestModel.Title), title: ErrorCodes.NotUniqueProperty.GetDescription());
                    var alreadyExistingEx = new AlreadyExistingObjectException<SeasonName>(error, $"There is already a {nameof(SeasonName)} with the same {nameof(Season)} and the same {nameof(SeasonName.Title)} value.");
                    throw alreadyExistingEx;
                }

                var rSeasonName = seasonNameRequestModel.ToSeasonName();
                seasonName.Title = rSeasonName.Title;

                seasonName = await seasonNameWriteRepo.UpdateSeasonName(seasonName);
                SeasonNameEditingResponseModel responseModel = seasonName.ToEditingResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(SeasonNameEditingResponseModel)}.{nameof(SeasonNameEditingResponseModel.Id)}: [{responseModel.Id}].");

                return responseModel;
            }
            catch (MismatchingIdException mismatchEx)
            {
                logger.Warning(mismatchEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{mismatchEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<SeasonName> ex)
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
                logger.Error($"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
        }
    }
}
