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
    public class EpisodeRatingReadRepository : IEpisodeRatingRead
    {
        private readonly ILogger logger = Log.ForContext<EpisodeRatingReadRepository>();
        private readonly AnimeBrowserContext abContext;

        public EpisodeRatingReadRepository(AnimeBrowserContext abContext)
        {
            this.abContext = abContext;
        }

        public EpisodeRating? GetEpisodeRatingByEpisodeAndUserId(long episodeId, string userId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(episodeId)}: [{episodeId}], {nameof(userId)}: [{userId}].");

            var episodeRating = abContext.EpisodeRatings.ToList().SingleOrDefault(er => er.EpisodeId == episodeId && er.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase));

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Found {nameof(episodeRating)}.{nameof(episodeRating.Id)}: [{episodeRating?.Id}].");
            return episodeRating;
        }

        public async Task<EpisodeRating?> GetEpisodeRatingById(long id)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(id)}: [{id}].");

            var episodeRating = await abContext.EpisodeRatings.FindAsync(id);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Found {nameof(episodeRating)}.{nameof(episodeRating.Id)}: [{episodeRating?.Id}].");
            return episodeRating;
        }

        public IEnumerable<EpisodeRating>? GetEpisodeRatingsByEpisodeId(long episodeId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(episodeId)}: [{episodeId}].");

            var episodeRatings = abContext.EpisodeRatings.ToList().Where(er => er.EpisodeId == episodeId);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(episodeRatings)}.Count: [{episodeRatings?.Count()}]");
            return episodeRatings;
        }

        public IEnumerable<EpisodeRating>? GetEpisodeRatingsByEpisodeIds(IEnumerable<long>? episodeIds)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(episodeIds)}.Count: [{episodeIds?.Count()}].");

            if (episodeIds?.Any() == true)
            {
                var episodeRatings = abContext.EpisodeRatings.ToList().Where(er => episodeIds.Contains(er.EpisodeId));

                logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(episodeRatings)}.Count: [{episodeRatings?.Count()}]");
                return episodeRatings;
            }

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. No ratings for empty {nameof(episodeIds)} list.");
            return Enumerable.Empty<EpisodeRating>();
        }
    }
}
