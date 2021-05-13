using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Write.MainInterfaces;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Write.MainRepositories
{
    public class SeasonWriteRepository : ISeasonWrite
    {
        private readonly AnimeBrowserContext abContext;
        private readonly ILogger logger = Log.ForContext<SeasonWriteRepository>();

        public SeasonWriteRepository(AnimeBrowserContext context)
        {
            abContext = context;
        }

        public async Task<Season> CreateSeason(Season season)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(Season)}: [{season}].");

            await abContext.AddAsync(season);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(Season)}.{nameof(Season.Id)}: [{season.Id}].");
            return season;
        }

        public async Task<Season> UpdateSeason(Season season)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(Season)}: [{season}].");

            abContext.Update(season);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(Season)}.{nameof(Season.Id)}: [{season.Id}].");
            return season;
        }

        public async Task DeleteSeason(Season season, IEnumerable<Episode>? episodes = null, IEnumerable<SeasonRating>? seasonRatings = null, IEnumerable<EpisodeRating>? episodeRatings = null)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(Season)}: [{season}].");

            if (episodeRatings?.Any() == true)
            {
                abContext.RemoveRange(episodeRatings);
            }
            if (episodes?.Any() == true)
            {
                abContext.RemoveRange(episodes);
            }
            if (seasonRatings?.Any() == true)
            {
                abContext.RemoveRange(seasonRatings);
            }
            abContext.Remove(season);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");
        }

        public async Task UpdateSeasonActiveStatus(Season season, IEnumerable<SeasonRating>? seasonRatings, IEnumerable<Episode>? episodes, IEnumerable<EpisodeRating>? episodeRatings)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(Season)}: [{season}].");

            if (episodeRatings?.Any() == true)
            {
                abContext.UpdateRange(episodeRatings);
            }
            if (episodes?.Any() == true)
            {
                abContext.UpdateRange(episodes);
            }
            if (seasonRatings?.Any() == true)
            {
                abContext.UpdateRange(seasonRatings);
            }
            abContext.Update(season);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");
        }
    }
}
