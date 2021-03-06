﻿using AnimeBrowser.BL.Helpers;
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
    public class SeasonEditingHandler : ISeasonEditing
    {
        private readonly ILogger logger = Log.ForContext<SeasonEditingHandler>();
        private readonly IDateTime dateTimeProvider;
        private readonly ISeasonRead seasonReadRepo;
        private readonly ISeasonWrite seasonWriteRepo;
        private readonly IAnimeInfoRead animeInfoReadRepo;

        public SeasonEditingHandler(IDateTime dateTimeProvider, ISeasonRead seasonReadRepo, ISeasonWrite seasonWriteRepo, IAnimeInfoRead animeInfoReadRepo)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.seasonReadRepo = seasonReadRepo;
            this.seasonWriteRepo = seasonWriteRepo;
            this.animeInfoReadRepo = animeInfoReadRepo;
        }

        public async Task<SeasonEditingResponseModel> EditSeason(long id, SeasonEditingRequestModel seasonRequestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with {nameof(id)}: [{id}], {nameof(seasonRequestModel)}: [{seasonRequestModel}].");

                if (id != seasonRequestModel?.Id)
                {
                    var error = new ErrorModel(code: ErrorCodes.MismatchingProperty.GetIntValueAsString(),
                       description: $"The parameter [{nameof(id)}] and [{nameof(seasonRequestModel)}.{nameof(seasonRequestModel.Id)}] properties should have the same value, but they are different!",
                       source: nameof(id), title: ErrorCodes.MismatchingProperty.GetDescription());
                    var mismatchEx = new MismatchingIdException(error, "The given id and the model's id are not matching!");
                    throw mismatchEx;
                }

                var validator = new SeasonEditingValidator(dateTimeProvider);
                var validationResult = await validator.ValidateAsync(seasonRequestModel);
                if (!validationResult.IsValid)
                {
                    var errorList = validationResult.Errors.ConvertToErrorModel();
                    throw new ValidationException(errorList, $"Validation error in [{nameof(SeasonEditingRequestModel)}].{Environment.NewLine}Validation errors: [{string.Join(", ", errorList)}].");
                }

                var isExistingSeasonWithSameSeasonNumber = seasonReadRepo.IsExistsSeasonWithSeasonNumber(seasonId: seasonRequestModel.Id, animeInfoId: seasonRequestModel.AnimeInfoId, seasonNumber: seasonRequestModel.SeasonNumber);
                if (isExistingSeasonWithSameSeasonNumber)
                {
                    var error = new ErrorModel(code: ErrorCodes.NotUniqueProperty.GetIntValueAsString(), description: $"Another {nameof(Season)} can be found in the same {nameof(AnimeInfo)} [{seasonRequestModel.AnimeInfoId}] " +
                        $"with the same {nameof(seasonRequestModel.SeasonNumber)} [{seasonRequestModel.SeasonNumber}].",
                        source: nameof(seasonRequestModel.SeasonNumber), title: ErrorCodes.NotUniqueProperty.GetDescription());
                    var alreadyExistingEx = new AlreadyExistingObjectException<Season>(error, $"There is already a {nameof(Season)} in the same {nameof(AnimeInfo)} with the same {nameof(Season.SeasonNumber)} value.");
                    throw alreadyExistingEx;
                }

                var season = await seasonReadRepo.GetSeasonById(id);
                if (season == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                        description: $"No {nameof(Season)} object was found with the given id [{id}]!",
                        source: nameof(seasonRequestModel.Id), title: ErrorCodes.EmptyObject.GetDescription()
                    );
                    var notExistingSeasonEx = new NotFoundObjectException<Season>(error, $"There is no {nameof(Season)} with given id: [{id}].");
                    throw notExistingSeasonEx;
                }

                var animeInfo = await animeInfoReadRepo.GetAnimeInfoById(seasonRequestModel.AnimeInfoId);
                if (animeInfo == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyProperty.GetIntValueAsString(),
                        description: $"The {nameof(AnimeInfo)} object is empty that is linked with the current {nameof(Season)} [{nameof(seasonRequestModel.AnimeInfoId)}: {seasonRequestModel.AnimeInfoId}]!",
                        source: nameof(seasonRequestModel.AnimeInfoId), title: ErrorCodes.EmptyProperty.GetDescription()
                    );
                    var notExistingAnimeInfoEx = new NotFoundObjectException<AnimeInfo>(error, $"There is no {nameof(AnimeInfo)} object that was given in {nameof(seasonRequestModel.AnimeInfoId)} property.");
                    throw notExistingAnimeInfoEx;
                }
                var rSeason = seasonRequestModel.ToSeason();
                season.UpdateSeasonWithOtherSeason(rSeason);

                season = await seasonWriteRepo.UpdateSeason(season);
                SeasonEditingResponseModel responseModel = season.ToEditingResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(SeasonEditingResponseModel)}.{nameof(SeasonEditingResponseModel.Id)}: [{responseModel.Id}].");

                return responseModel;
            }
            catch (MismatchingIdException mismatchEx)
            {
                logger.Warning(mismatchEx, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{mismatchEx.Message}].");
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
            catch (NotFoundObjectException<Season> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (NotFoundObjectException<AnimeInfo> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
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
