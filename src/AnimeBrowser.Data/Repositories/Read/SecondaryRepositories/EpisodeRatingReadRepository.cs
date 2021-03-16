using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
