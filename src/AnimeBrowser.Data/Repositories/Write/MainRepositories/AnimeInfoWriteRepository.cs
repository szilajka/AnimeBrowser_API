using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Write.MainInterfaces;
using Serilog;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Write.MainRepositories
{
    public class AnimeInfoWriteRepository : IAnimeInfoWrite
    {
        private readonly AnimeBrowserContext context;
        private readonly ILogger logger = Log.ForContext<AnimeInfoWriteRepository>();

        public AnimeInfoWriteRepository(AnimeBrowserContext context)
        {
            this.context = context;
        }

        public async Task<AnimeInfo> CreateAnimeInfo(AnimeInfo animeInfo)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(AnimeInfo)}: [{animeInfo}].");

            await context.AddAsync(animeInfo);
            await context.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(AnimeInfo.Id)}: [{animeInfo.Id}].");
            return animeInfo;
        }

        public async Task<AnimeInfo> UpdateAnimeInfo(AnimeInfo animeInfo)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(AnimeInfo)}: [{animeInfo}].");

            context.Update(animeInfo);
            await context.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(AnimeInfo)}.{nameof(AnimeInfo.Id)}: [{animeInfo.Id}].");
            return animeInfo;
        }

        public async Task DeleteAnimeInfo(AnimeInfo animeInfo)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(AnimeInfo)}: [{animeInfo}].");

            context.Remove(animeInfo);
            await context.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");
        }
    }
}
