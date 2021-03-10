using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Write.SecondaryInterfaces;
using Serilog;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Write.SecondaryRepositories
{
    public class SeasonNameWriteRepository : ISeasonNameWrite
    {
        private readonly ILogger logger = Log.ForContext<SeasonNameWriteRepository>();
        private readonly AnimeBrowserContext abContext;

        public SeasonNameWriteRepository(AnimeBrowserContext abContext)
        {
            this.abContext = abContext;
        }

        public async Task<SeasonName> CreateSeasonName(SeasonName seasonName)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(SeasonName)}: [{seasonName}].");

            await abContext.AddAsync(seasonName);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(SeasonName.Id)}: [{seasonName.Id}].");
            return seasonName;
        }
    }
}
