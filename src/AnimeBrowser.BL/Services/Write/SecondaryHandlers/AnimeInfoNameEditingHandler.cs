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
    public class AnimeInfoNameEditingHandler : IAnimeInfoNameEditing
    {
        private readonly ILogger logger = Log.ForContext<AnimeInfoNameEditingHandler>();
        private readonly IAnimeInfoNameWrite animeInfoNameWriteRepo;
        private readonly IAnimeInfoNameRead animeInfoNameReadRepo;
        private readonly IAnimeInfoRead animeInfoReadRepo;

        public AnimeInfoNameEditingHandler(IAnimeInfoNameWrite animeInfoNameWriteRepo, IAnimeInfoNameRead animeInfoNameReadRepo, IAnimeInfoRead animeInfoReadRepo)
        {
            this.animeInfoNameWriteRepo = animeInfoNameWriteRepo;
            this.animeInfoNameReadRepo = animeInfoNameReadRepo;
            this.animeInfoReadRepo = animeInfoReadRepo;
        }

        public async Task<AnimeInfoNameEditingResponseModel> EditAnimeInfoName(long id, AnimeInfoNameEditingRequestModel animeInfoNameRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with {nameof(id)}: [{id}], {nameof(animeInfoNameRequestModel)}: [{animeInfoNameRequestModel}].");

                if (id != animeInfoNameRequestModel?.Id)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                       description: $"The parameter [{nameof(id)}] and [{nameof(animeInfoNameRequestModel)}.{nameof(animeInfoNameRequestModel.Id)}] properties should have the same value, but they are different!",
                       source: nameof(id), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var mismatchEx = new MismatchingIdException(error, "The given id and the model's id are not matching!");
                    throw mismatchEx;
                }

                var animeInfoName = await animeInfoNameReadRepo.GetAnimeInfoNameById(id);
                if (animeInfoName == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                        description: $"No {nameof(AnimeInfoName)} object was found with the given id [{id}]!",
                        source: nameof(id), title: ErrorCodes.EmptyObject.GetDescription()
                    );
                    var notExistingAnimeInfoNameEx = new NotFoundObjectException<AnimeInfoName>(error, $"There is no {nameof(AnimeInfoName)} with given id: [{id}].");
                    throw notExistingAnimeInfoNameEx;
                }

                var animeInfo = await animeInfoReadRepo.GetAnimeInfoById(animeInfoNameRequestModel.AnimeInfoId);
                if (animeInfo == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyProperty.GetIntValueAsString(),
                        description: $"The {nameof(AnimeInfo)} object is empty that is linked with the current {nameof(AnimeInfoName)} [{nameof(animeInfoNameRequestModel.AnimeInfoId)}: {animeInfoNameRequestModel.AnimeInfoId}]!",
                        source: nameof(animeInfoNameRequestModel.AnimeInfoId), title: ErrorCodes.EmptyProperty.GetDescription()
                    );
                    var notExistingAnimeInfoEx = new NotFoundObjectException<AnimeInfo>(error, $"There is no {nameof(AnimeInfo)} object that was given in {nameof(animeInfoNameRequestModel.AnimeInfoId)} property.");
                    throw notExistingAnimeInfoEx;
                }

                var validator = new AnimeInfoNameEditingValidator();
                var validationResult = await validator.ValidateAsync(animeInfoNameRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(AnimeInfoNameEditingRequestModel)}].{Environment.NewLine}Validation errors: [{string.Join(", ", errorList)}].");
                }

                animeInfoNameRequestModel.Title = animeInfoNameRequestModel.Title.Trim();
                var isExistingWithSameTitle = animeInfoNameReadRepo.IsExistingWithSameTitle(animeInfoNameRequestModel.Id, animeInfoNameRequestModel.Title, animeInfoNameRequestModel.AnimeInfoId);
                if (isExistingWithSameTitle || animeInfo.Title.Equals(animeInfoNameRequestModel.Title, StringComparison.OrdinalIgnoreCase))
                {
                    var error = new ErrorModel(code: ErrorCodes.NotUniqueProperty.GetIntValueAsString(), description: $"Another {nameof(AnimeInfoName)} can be found with the same {nameof(AnimeInfo)} [{animeInfoNameRequestModel.AnimeInfoId}] " +
                       $"and the same {nameof(animeInfoNameRequestModel.Title)} [{animeInfoNameRequestModel.Title}].",
                       source: nameof(animeInfoNameRequestModel.Title), title: ErrorCodes.NotUniqueProperty.GetDescription());
                    var alreadyExistingEx = new AlreadyExistingObjectException<AnimeInfoName>(error, $"There is already a {nameof(AnimeInfoName)} with the same {nameof(AnimeInfo)} and the same {nameof(AnimeInfoName.Title)} value.");
                    throw alreadyExistingEx;
                }

                var rAnimeInfoName = animeInfoNameRequestModel.ToAnimeInfoName();
                animeInfoName.Title = rAnimeInfoName.Title;

                animeInfoName = await animeInfoNameWriteRepo.UpdateAnimeInfoName(animeInfoName);
                AnimeInfoNameEditingResponseModel responseModel = animeInfoName.ToEditingResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(AnimeInfoNameEditingResponseModel)}.{nameof(AnimeInfoNameEditingResponseModel.Id)}: [{responseModel.Id}].");

                return responseModel;
            }
            catch (MismatchingIdException mismatchEx)
            {
                logger.Warning(mismatchEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{mismatchEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<AnimeInfoName> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (NotFoundObjectException<AnimeInfo> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
                throw;
            }
            catch (AlreadyExistingObjectException<AnimeInfoName> alreadyExistingEx)
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
