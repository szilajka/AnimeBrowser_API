using AnimeBrowser.BL.Interfaces.Write.MainInterfaces;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.MainInterfaces;
using AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces;
using AnimeBrowser.Data.Interfaces.Write.MainInterfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write.MainHandlers
{
    public class SeasonInactivationHandler : ISeasonInactivation
    {
        private readonly ILogger logger = Log.ForContext<SeasonInactivationHandler>();
        private readonly ISeasonRead seasonReadRepo;
        private readonly ISeasonRatingRead seasonRatingReadRepo;
        private readonly IEpisodeRead episodeReadRepo;
        private readonly IEpisodeRatingRead episodeRatingReadRepo;
        private readonly ISeasonWrite seasonWriteRepo;

        public SeasonInactivationHandler(ISeasonRead seasonReadRepo, ISeasonRatingRead seasonRatingReadRepo, IEpisodeRead episodeReadRepo, IEpisodeRatingRead episodeRatingReadRepo,
            ISeasonWrite seasonWriteRepo)
        {
            this.seasonReadRepo = seasonReadRepo;
            this.seasonRatingReadRepo = seasonRatingReadRepo;
            this.episodeReadRepo = episodeReadRepo;
            this.episodeRatingReadRepo = episodeRatingReadRepo;
            this.seasonWriteRepo = seasonWriteRepo;
        }

        public async Task<Season> Inactivate(long seasonId)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. [{nameof(seasonId)}]: [{seasonId}].");

                if (seasonId <= 0)
                {
                    var error = new ErrorModel(code: ErrorCodes.OutOfRangeProperty.GetIntValueAsString(), description: $"The given id [{seasonId}] is not a valid id. A valid id must be greater than 0!",
                        source: nameof(seasonId), title: ErrorCodes.OutOfRangeProperty.GetDescription());
                    throw new NotExistingIdException(error, $"The given {nameof(Season)}'s id is less than/equal to 0!");
                }

                var season = await seasonReadRepo.GetSeasonById(seasonId);
                if (season == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                        description: $"No {nameof(Season)} object was found with the given id [{seasonId}]!",
                        source: nameof(seasonId), title: ErrorCodes.EmptyObject.GetDescription()
                    );
                    throw new NotFoundObjectException<Season>(error, $"Not found an {nameof(Season)} entity with id: [{seasonId}].");
                }

                var seasonRatings = seasonRatingReadRepo.GetSeasonRatingsBySeasonId(seasonId);
                var episodes = episodeReadRepo.GetEpisodesBySeasonId(seasonId);
                var episodeIds = episodes?.Select(e => e.Id)?.Distinct();
                var episodeRatings = episodeRatingReadRepo.GetEpisodeRatingsByEpisodeIds(episodeIds);
                if (episodeRatings?.Any() == true)
                {
                    foreach (var er in episodeRatings)
                    {
                        er.IsSeasonActive = false;
                    }
                }
                if (episodes?.Any() == true)
                {
                    foreach (var e in episodes)
                    {
                        e.IsSeasonActive = false;
                    }
                }
                if (seasonRatings?.Any() == true)
                {
                    foreach (var sr in seasonRatings)
                    {
                        sr.IsSeasonActive = false;
                    }
                }
                season.IsActive = false;

                seasonWriteRepo.UpdateSeasonActiveStatus(season, seasonRatings, episodes, episodeRatings);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");
                return season;
            }
            catch (NotExistingIdException ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (NotFoundObjectException<Season> ex)
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
