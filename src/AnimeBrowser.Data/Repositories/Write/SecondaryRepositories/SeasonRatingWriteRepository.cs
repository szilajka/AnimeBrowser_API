using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Write.SecondaryInterfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Write.SecondaryRepositories
{
    public class SeasonRatingWriteRepository : ISeasonRatingWrite
    {
        private readonly ILogger logger = Log.ForContext<SeasonRatingWriteRepository>();
        private readonly AnimeBrowserContext abContext;

        public SeasonRatingWriteRepository(AnimeBrowserContext abContext)
        {
            this.abContext = abContext;
        }

        public async Task<SeasonRating> CreateSeasonRating(SeasonRating seasonRating)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(SeasonRating)}: [{seasonRating}].");

            await abContext.AddAsync(seasonRating);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(SeasonRating.Id)}: [{seasonRating.Id}].");
            return seasonRating;
        }
    }
}
