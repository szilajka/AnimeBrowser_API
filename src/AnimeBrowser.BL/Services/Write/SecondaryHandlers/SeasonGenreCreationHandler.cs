using AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write.SecondaryHandlers
{
    public class SeasonGenreCreationHandler : ISeasonGenreCreation
    {
        private readonly ILogger logger = Log.ForContext<SeasonGenreCreationHandler>();
        private readonly ISeasonRead seasonReadRepo;
        private readonly IGenreRead genreReadRepo;
        private readonly ISeasonGenreRead seasonGenreReadRepo;
        private readonly ISeasonGenreWrite seasonGenreWriteRepo;

        public SeasonGenreCreationHandler(ISeasonRead seasonReadRepo, IGenreRead genreReadRepo, ISeasonGenreRead seasonGenreReadRepo, ISeasonGenreWrite seasonGenreWriteRepo)
        {
            this.seasonReadRepo = seasonReadRepo;
            this.genreReadRepo = genreReadRepo;
            this.seasonGenreReadRepo = seasonGenreReadRepo;
            this.seasonGenreWriteRepo = seasonGenreWriteRepo;
        }

        public async Task<IEnumerable<SeasonGenreCreationResponseModel>> CreateSeasonGenres(IList<SeasonGenreCreationRequestModel> requestModel)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with request model: [{string.Join(", ", requestModel ?? new List<SeasonGenreCreationRequestModel>())}].");
                if (requestModel?.Any() != true)
                {
                    var errorModel = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(), description: $"The {nameof(SeasonGenreCreationRequestModel)} list is empty!",
                        source: nameof(requestModel), title: ErrorCodes.EmptyObject.GetDescription());
                    throw new EmptyObjectException<SeasonGenreCreationRequestModel>(errorModel, $"The given {nameof(SeasonGenreCreationRequestModel)} list is empty!");
                }

                var seasonIds = requestModel.Select(sg => sg.SeasonId).Distinct();
                if (seasonIds.Any(id => id <= 0))
                {
                    var emptyRequestModels = requestModel.Where(sg => sg.SeasonId <= 0).ToList();
                    var error = new ErrorModel(code: ErrorCodes.OutOfRangeProperty.GetIntValueAsString(), description: $"The given [{string.Join(", ", emptyRequestModels)}] objects' {nameof(SeasonGenreCreationRequestModel.SeasonId)} " +
                        $"are not valid ids. A valid id must be greater than 0!",
                       source: nameof(SeasonGenreCreationRequestModel.SeasonId), title: ErrorCodes.OutOfRangeProperty.GetDescription());
                    throw new NotExistingIdException(error, $"The given {nameof(SeasonGenreCreationRequestModel)}'s {nameof(SeasonGenreCreationRequestModel.SeasonId)} is less than/equal to 0!");
                }

                var genreIds = requestModel.Select(sg => sg.GenreId).Distinct();
                if (genreIds.Any(id => id <= 0))
                {
                    var emptyRequestModels = requestModel.Where(sg => sg.GenreId <= 0).ToList();
                    var error = new ErrorModel(code: ErrorCodes.OutOfRangeProperty.GetIntValueAsString(), description: $"The given [{string.Join(", ", emptyRequestModels)}] objects' {nameof(SeasonGenreCreationRequestModel.GenreId)} " +
                        $"are not valid ids. A valid id must be greater than 0!",
                       source: nameof(SeasonGenreCreationRequestModel.GenreId), title: ErrorCodes.OutOfRangeProperty.GetDescription());
                    throw new NotExistingIdException(error, $"The given {nameof(SeasonGenreCreationRequestModel)}'s {nameof(SeasonGenreCreationRequestModel.GenreId)} is less than/equal to 0!");
                }

                var seasons = seasonReadRepo.GetSeasonsByIds(seasonIds);
                if (seasons == null || seasons.Count == 0 || seasons.Count != seasonIds.Count())
                {
                    ErrorModel error;
                    if (seasons?.Count > 0 && seasonIds.Count() != seasons.Count)
                    {
                        var existingSeasonIds = seasons.Select(s => s.Id).Distinct().ToList();
                        var emptyRequestModels = requestModel.Where(sg => !existingSeasonIds.Contains(sg.SeasonId));
                        error = new ErrorModel(code: ErrorCodes.EmptyProperty.GetIntValueAsString(),
                            description: $"No {nameof(Season)} object was found with the {nameof(SeasonGenre)}s' {nameof(SeasonGenreCreationRequestModel.SeasonId)}s [{string.Join(", ", emptyRequestModels)}]!",
                            source: nameof(SeasonGenreCreationRequestModel.SeasonId), title: ErrorCodes.EmptyProperty.GetDescription()
                        );
                    }
                    else
                    {
                        error = new ErrorModel(code: ErrorCodes.EmptyProperty.GetIntValueAsString(),
                            description: $"No {nameof(Season)} object was found with the {nameof(SeasonGenre)}s' {nameof(SeasonGenreCreationRequestModel.SeasonId)}s!",
                            source: nameof(SeasonGenreCreationRequestModel.SeasonId), title: ErrorCodes.EmptyProperty.GetDescription()
                        );
                    }
                    var notExistingSeasonEx = new NotFoundObjectException<Season>(error, $"There is no {nameof(Season)} object that was given in {nameof(SeasonGenreCreationRequestModel.SeasonId)} property.");
                    throw notExistingSeasonEx;
                }

                var genres = genreReadRepo.GetGenresByIds(genreIds);
                if (genres == null || genres.Count == 0 || genres.Count != genreIds.Count())
                {
                    ErrorModel error;
                    if (genres?.Count > 0 && genreIds.Count() != genres.Count)
                    {
                        var existingGenreIds = genres.Select(s => s.Id).Distinct().ToList();
                        var emptyRequestModels = requestModel.Where(sg => !existingGenreIds.Contains(sg.GenreId));
                        error = new ErrorModel(code: ErrorCodes.EmptyProperty.GetIntValueAsString(),
                            description: $"No {nameof(Genre)} object was found with the {nameof(SeasonGenre)}s' {nameof(SeasonGenreCreationRequestModel.GenreId)}s [{string.Join(", ", emptyRequestModels)}]!",
                            source: nameof(SeasonGenreCreationRequestModel.GenreId), title: ErrorCodes.EmptyProperty.GetDescription()
                        );
                    }
                    else
                    {
                        error = new ErrorModel(code: ErrorCodes.EmptyProperty.GetIntValueAsString(),
                            description: $"No {nameof(Genre)} object was found with the {nameof(SeasonGenre)}s' {nameof(SeasonGenreCreationRequestModel.GenreId)}s!",
                            source: nameof(SeasonGenreCreationRequestModel.GenreId), title: ErrorCodes.EmptyProperty.GetDescription()
                        );
                    }
                    var notExistingGenreEx = new NotFoundObjectException<Genre>(error, $"There is no {nameof(Genre)} object that was given in {nameof(SeasonGenreCreationRequestModel.GenreId)} property.");
                    throw notExistingGenreEx;
                }

                var seasonGenres = seasonGenreReadRepo.GetSeasonGenreBySeasonAndGenreIds(requestModel.Select(sg => (SeasonId: sg.SeasonId, GenreId: sg.GenreId)).ToList());
                if (seasonGenres.Count > 0)
                {
                    List<SeasonGenreCreationRequestModel> removableRequestModels = new();
                    foreach (var sg in seasonGenres)
                    {
                        var removableSgs = requestModel.Where(rm => rm.GenreId == sg.GenreId && rm.SeasonId == sg.SeasonId);
                        if (removableSgs.Any())
                        {
                            removableRequestModels.AddRange(removableSgs);
                        }
                    }
                    foreach (var rsg in removableRequestModels)
                    {
                        requestModel.Remove(rsg);
                    }
                }

                var insertableSeasonGenres = requestModel.ToSeasonGenres();

                var createdSeasonGenres = await seasonGenreWriteRepo.CreateSeasonGenres(insertableSeasonGenres);
                var responseModel = createdSeasonGenres.ToCreationResponseModel();

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(SeasonGenreCreationResponseModel)}.Count: [{responseModel.Count}]");
                return responseModel;
            }
            catch (EmptyObjectException<SeasonGenreCreationRequestModel> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (NotExistingIdException noIdEx)
            {
                logger.Warning(noIdEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{noIdEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<Season> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (NotFoundObjectException<Genre> ex)
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
