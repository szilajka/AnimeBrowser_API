using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Read.SecondaryRepositories
{
    public class SeasonNameReadRepository : ISeasonNameRead
    {
        private readonly ILogger logger = Log.ForContext<SeasonNameReadRepository>();
        private readonly AnimeBrowserContext abContext;

        public SeasonNameReadRepository(AnimeBrowserContext abContext)
        {
            this.abContext = abContext;
        }

        public async Task<SeasonName?> GetSeasonNameById(long id)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(id)}: [{id}].");

            var seasonName = await abContext.SeasonNames.FindAsync(id);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Found {nameof(seasonName)}.{nameof(seasonName.Id)}: [{seasonName?.Id}].");
            return seasonName;
        }

        public bool IsExistsWithSameTitle(long id, string title, long seasonId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(id)}: [{id}], {nameof(title)}: [{title}], {nameof(seasonId)}: [{seasonId}].");

            var isExistsWithSameTitle = abContext.SeasonNames.ToList().Any(ain => ain.Id != id && ain.SeasonId == seasonId && ain.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Found {nameof(isExistsWithSameTitle)}: [{isExistsWithSameTitle}].");
            return isExistsWithSameTitle;
        }

        public bool IsExistsWithSameTitle(string title, long seasonId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(title)}: [{title}], {nameof(seasonId)}: [{seasonId}].");

            var isExistsWithSameTitle = abContext.SeasonNames.ToList().Any(ain => ain.SeasonId == seasonId && ain.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Found {nameof(isExistsWithSameTitle)}: [{isExistsWithSameTitle}].");
            return isExistsWithSameTitle;
        }
    }
}
