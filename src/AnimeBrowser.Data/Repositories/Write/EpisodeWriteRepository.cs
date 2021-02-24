using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Write;
using Serilog;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Write
{
    public class EpisodeWriteRepository : IEpisodeWrite
    {
        private readonly AnimeBrowserContext abContext;
        private readonly ILogger logger = Log.ForContext<EpisodeWriteRepository>();

        public EpisodeWriteRepository(AnimeBrowserContext context)
        {
            this.abContext = context;
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
    }
}
