using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Write.MainInterfaces;
using Serilog;
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

        public async Task DeleteSeason(Season season)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(Season)}: [{season}].");

            abContext.Remove(season);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");
        }
    }
}
