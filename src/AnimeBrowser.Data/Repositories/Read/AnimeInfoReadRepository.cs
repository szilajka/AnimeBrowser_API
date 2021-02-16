using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read;
using Serilog;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Read
{
    public class AnimeInfoReadRepository : IAnimeInfoRead
    {
        private readonly AnimeBrowserContext abContext;
        private readonly ILogger logger = Log.ForContext<AnimeInfoReadRepository>();

        public AnimeInfoReadRepository(AnimeBrowserContext abContext)
        {
            this.abContext = abContext;
        }

        public async Task<AnimeInfo> GetAnimeInfoById(long id)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. id: [{id}].");

            var animeInfo = await abContext.AnimeInfos.FindAsync(id);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Found animeInfo?.Id: [{animeInfo?.Id}].");
            return animeInfo;
        }
    }
}
