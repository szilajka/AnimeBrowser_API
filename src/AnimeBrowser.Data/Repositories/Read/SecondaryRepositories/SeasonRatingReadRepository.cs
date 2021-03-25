using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Read.SecondaryRepositories
{
    public class SeasonRatingReadRepository : ISeasonRatingRead
    {
        private readonly ILogger logger = Log.ForContext<SeasonRatingReadRepository>();
        private readonly AnimeBrowserContext abContext;

        public SeasonRatingReadRepository(AnimeBrowserContext abContext)
        {
            this.abContext = abContext;
        }

        public async Task<SeasonRating?> GetSeasonRatingById(long id)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(id)}: [{id}].");

            var seasonRating = await abContext.SeasonRatings.FindAsync(id);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Found {nameof(seasonRating)}.{nameof(seasonRating.Id)}: [{seasonRating?.Id}].");
            return seasonRating;
        }

        public SeasonRating? GetSeasonRatingBySeasonAndUserId(long seasonId, string userId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonId)}: [{seasonId}], {nameof(userId)}: [{userId}].");

            var seasonRating = abContext.SeasonRatings.ToList().SingleOrDefault(sr => sr.SeasonId == seasonId && sr.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase));

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Found {nameof(seasonRating)}.{nameof(seasonRating.Id)}: [{seasonRating?.Id}].");
            return seasonRating;
        }

        public IEnumerable<SeasonRating>? GetSeasonRatingsBySeasonId(long seasonId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonId)}: [{seasonId}].");

            var seasonRatings = abContext.SeasonRatings.ToList().Where(sr => sr.SeasonId == seasonId);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(seasonRatings)}.Count: [{seasonRatings?.Count()}].");
            return seasonRatings;
        }

        public IEnumerable<SeasonRating>? GetSeasonRatingsBySeasonIds(IEnumerable<long> seasonIds)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonIds)}: [{string.Join(", ", seasonIds)}].");

            var seasonRatings = abContext.SeasonRatings.ToList().Where(sr => seasonIds.Contains(sr.SeasonId));

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(seasonRatings)}.Count: [{seasonRatings?.Count()}].");
            return seasonRatings;
        }
    }
}
