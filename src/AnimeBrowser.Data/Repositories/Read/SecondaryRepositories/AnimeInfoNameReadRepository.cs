using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Read.SecondaryRepositories
{
    public class AnimeInfoNameReadRepository : IAnimeInfoNameRead
    {
        private readonly ILogger logger = Log.ForContext<AnimeInfoNameReadRepository>();
        private readonly AnimeBrowserContext abContext;

        public AnimeInfoNameReadRepository(AnimeBrowserContext abContext)
        {
            this.abContext = abContext;
        }

        public async Task<AnimeInfoName?> GetAnimeInfoNameById(long id)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(id)}: [{id}].");

            var animeInfoName = await abContext.AnimeInfoNames.FindAsync(id);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Found {nameof(animeInfoName)}.{nameof(animeInfoName.Id)}: [{animeInfoName?.Id}].");
            return animeInfoName;
        }

        public bool IsExistingWithSameTitle(long id, string title, long animeInfoId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(title)}: [{title}].");

            var isExistsWithSameTitle = abContext.AnimeInfoNames.ToList().Any(ain => ain.Id != id && ain.AnimeInfoId == animeInfoId && ain.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Found {nameof(isExistsWithSameTitle)}: [{isExistsWithSameTitle}].");
            return isExistsWithSameTitle;
        }

        public bool IsExistingWithSameTitle(string title, long animeInfoId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(title)}: [{title}].");

            var isExistsWithSameTitle = abContext.AnimeInfoNames.ToList().Any(ain => ain.AnimeInfoId == animeInfoId && ain.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Found {nameof(isExistsWithSameTitle)}: [{isExistsWithSameTitle}].");
            return isExistsWithSameTitle;
        }
    }
}
