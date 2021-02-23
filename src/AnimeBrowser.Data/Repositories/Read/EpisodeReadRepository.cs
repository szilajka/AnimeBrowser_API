using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read;
using Serilog;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Read
{
    public class EpisodeReadRepository : IEpisodeRead
    {
        private readonly AnimeBrowserContext abContext;
        private readonly ILogger logger = Log.ForContext<EpisodeReadRepository>();

        public EpisodeReadRepository(AnimeBrowserContext context)
        {
            this.abContext = context;
        }

        public async Task<Episode?> GetEpisodeById(long episodeId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(episodeId)}: [{episodeId}].");

            var episode = await abContext.Episodes.FindAsync(episodeId);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(Episode)}?.{nameof(Episode.Id)}: [{episode?.Id}].");
            return episode;
        }

        public async Task<bool> IsSeasonAndAnimeInfoExistsAndReferences(long seasonId, long animeInfoId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonId)}: [{seasonId}], {nameof(animeInfoId)}: [{animeInfoId}].");
            var result = false;
            var season = await abContext.Seasons.FindAsync(seasonId);
            if (season == null || season.AnimeInfoId != animeInfoId) return result;
            var animeInfo = await abContext.AnimeInfos.FindAsync(animeInfoId);
            if (animeInfo == null) return result;

            result = true;
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. result: [{result}].");
            return result;
        }
    }
}
