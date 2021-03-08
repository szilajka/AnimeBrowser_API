using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.MainInterfaces;
using Serilog;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Read.MainRepositories
{
    public class AnimeInfoReadRepository : IAnimeInfoRead
    {
        private readonly AnimeBrowserContext abContext;
        private readonly ILogger logger = Log.ForContext<AnimeInfoReadRepository>();

        public AnimeInfoReadRepository(AnimeBrowserContext abContext)
        {
            this.abContext = abContext;
        }

        public async Task<AnimeInfo?> GetAnimeInfoById(long id)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(id)}: [{id}].");

            var animeInfo = await abContext.AnimeInfos.FindAsync(id);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Found {nameof(animeInfo)}.{nameof(animeInfo.Id)}: [{animeInfo?.Id}].");
            return animeInfo;
        }
    }
}
