using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read;
using Serilog;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Read
{
    public class SeasonReadRepository : ISeasonRead
    {
        private readonly AnimeBrowserContext abContext;
        private readonly ILogger logger = Log.ForContext<SeasonReadRepository>();

        public SeasonReadRepository(AnimeBrowserContext context)
        {
            this.abContext = context;
        }

        public async Task<Season?> GetSeasonById(long seasonId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonId)}: [{seasonId}].");

            var season = await abContext.Seasons.FindAsync(seasonId);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(Season)}?.{nameof(Season.Id)}: [{season?.Id}].");
            return season;
        }
    }
}
