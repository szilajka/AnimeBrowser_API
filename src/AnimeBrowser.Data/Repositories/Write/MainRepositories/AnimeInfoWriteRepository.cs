using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Write.MainInterfaces;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Write.MainRepositories
{
    public class AnimeInfoWriteRepository : IAnimeInfoWrite
    {
        private readonly AnimeBrowserContext abContext;
        private readonly ILogger logger = Log.ForContext<AnimeInfoWriteRepository>();

        public AnimeInfoWriteRepository(AnimeBrowserContext context)
        {
            this.abContext = context;
        }

        public async Task<AnimeInfo> CreateAnimeInfo(AnimeInfo animeInfo)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(AnimeInfo)}: [{animeInfo}].");

            await abContext.AddAsync(animeInfo);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(AnimeInfo.Id)}: [{animeInfo.Id}].");
            return animeInfo;
        }

        public async Task<AnimeInfo> UpdateAnimeInfo(AnimeInfo animeInfo)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(AnimeInfo)}: [{animeInfo}].");

            abContext.Update(animeInfo);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(AnimeInfo)}.{nameof(AnimeInfo.Id)}: [{animeInfo.Id}].");
            return animeInfo;
        }

        public async Task DeleteAnimeInfo(AnimeInfo animeInfo, IEnumerable<Season>? seasons = null, IEnumerable<Episode>? episodes = null,
            IEnumerable<SeasonRating>? seasonRatings = null, IEnumerable<EpisodeRating>? episodeRatings = null)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(AnimeInfo)}: [{animeInfo}].");

            if (episodeRatings?.Any() == true)
            {
                abContext.RemoveRange(episodeRatings);
            }
            if (seasonRatings?.Any() == true)
            {
                abContext.RemoveRange(seasonRatings);
            }
            if (episodes?.Any() == true)
            {
                abContext.RemoveRange(episodes);
            }
            if (seasons?.Any() == true)
            {
                abContext.RemoveRange(seasons);
            }
            abContext.Remove(animeInfo);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");
        }
    }
}
