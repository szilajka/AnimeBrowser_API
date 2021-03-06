﻿using AnimeBrowser.BL.Helpers;
using AnimeBrowser.BL.Interfaces.Write.MainInterfaces;
using AnimeBrowser.BL.Validators.MainValidators;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Common.Models.RequestModels.MainModels;
using AnimeBrowser.Common.Models.ResponseModels.MainModels;
using AnimeBrowser.Data.Converters.MainConverters;
using AnimeBrowser.Data.Interfaces.Write.MainInterfaces;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write.MainHandlers
{
    public class AnimeInfoCreationHandler : IAnimeInfoCreation
    {
        private readonly ILogger logger = Log.ForContext<AnimeInfoCreationHandler>();
        private readonly IAnimeInfoWrite animeInfoWriteRepo;

        public AnimeInfoCreationHandler(IAnimeInfoWrite animeInfoWriteRepo)
        {
            this.animeInfoWriteRepo = animeInfoWriteRepo;
        }

        public async Task<AnimeInfoCreationResponseModel> CreateAnimeInfo(AnimeInfoCreationRequestModel animeInfoRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. [{nameof(animeInfoRequestModel)}]: [{animeInfoRequestModel}].");
                if (animeInfoRequestModel == null)
                {
                    var errorModel = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(), description: $"The {nameof(AnimeInfoCreationRequestModel)} object is empty!",
                        source: nameof(animeInfoRequestModel), title: ErrorCodes.EmptyObject.GetDescription());
                    throw new EmptyObjectException<AnimeInfoCreationRequestModel>(errorModel, $"The given {nameof(AnimeInfoCreationRequestModel)} object is empty!");
                }

                var validator = new AnimeInfoCreationValidator();
                var validationResult = await validator.ValidateAsync(animeInfoRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(AnimeInfoCreationRequestModel)}].{Environment.NewLine}Validation errors: [{string.Join(", ", errorList)}].");
                }

                var animeInfo = await animeInfoWriteRepo.CreateAnimeInfo(animeInfoRequestModel.ToAnimeInfo());
                var responseModel = animeInfo.ToCreationResponseModel();
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(AnimeInfoCreationResponseModel)}.{nameof(AnimeInfoCreationResponseModel.Id)}: [{responseModel.Id}]");
                return responseModel;
            }
            catch (EmptyObjectException<AnimeInfoCreationRequestModel> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (ValidationException valEx)
            {
                logger.Warning(valEx, $"Validation error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{valEx.Message}].");
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
