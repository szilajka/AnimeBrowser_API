using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Write.MainInterfaces;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Write.MainRepositories
{
    public class EpisodeWriteRepository : IEpisodeWrite
    {
        private readonly AnimeBrowserContext abContext;
        private readonly ILogger logger = Log.ForContext<EpisodeWriteRepository>();

        public EpisodeWriteRepository(AnimeBrowserContext context)
        {
            abContext = context;
        }

        public async Task<Episode> CreateEpisode(Episode episode)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(Episode)}: [{episode}].");

            await abContext.AddAsync(episode);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(Episode)}.{nameof(Episode.Id)}: [{episode.Id}].");
            return episode;
        }

        public async Task<Episode> UpdateEpisode(Episode episode)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(Episode)}: [{episode}].");

            abContext.Update(episode);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(Episode)}.{nameof(Episode.Id)}: [{episode.Id}].");
            return episode;
        }

        public async Task DeleteEpisode(Episode episode, IEnumerable<EpisodeRating>? episodeRatings = null)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(Episode)}: [{episode}], {nameof(episodeRatings)}.Count: [{episodeRatings?.Count()}].");

            if (episodeRatings?.Any() == true)
            {
                abContext.RemoveRange(episodeRatings);
            }
            abContext.Remove(episode);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");
        }
    }
}
