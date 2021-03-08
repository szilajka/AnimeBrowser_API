using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Write.SecondaryInterfaces;
using Serilog;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Write.SecondaryRepositories
{
    public class AnimeInfoNameWriteRepository : IAnimeInfoNameWrite
    {
        private readonly ILogger logger = Log.ForContext<AnimeInfoNameWriteRepository>();
        private readonly AnimeBrowserContext abContext;

        public AnimeInfoNameWriteRepository(AnimeBrowserContext abContext)
        {
            this.abContext = abContext;
        }

        public async Task<AnimeInfoName> CreateAnimeInfoName(AnimeInfoName animeInfoName)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(AnimeInfoName)}: [{animeInfoName}].");

            await abContext.AddAsync(animeInfoName);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(AnimeInfoName.Id)}: [{animeInfoName.Id}].");
            return animeInfoName;
        }
    }
}
