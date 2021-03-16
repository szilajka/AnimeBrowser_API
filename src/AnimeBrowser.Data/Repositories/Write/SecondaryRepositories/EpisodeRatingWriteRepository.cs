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
    public class EpisodeRatingWriteRepository : IEpisodeRatingWrite
    {

        private readonly ILogger logger = Log.ForContext<EpisodeRatingWriteRepository>();
        private readonly AnimeBrowserContext abContext;

        public EpisodeRatingWriteRepository(AnimeBrowserContext abContext)
        {
            this.abContext = abContext;
        }

        public async Task<EpisodeRating> CreateEpisodeRating(EpisodeRating episodeRating)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(EpisodeRating)}: [{episodeRating}].");

            await abContext.AddAsync(episodeRating);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(EpisodeRating.Id)}: [{episodeRating.Id}].");
            return episodeRating;
        }

        public async Task<EpisodeRating> UpdateEpisodeRating(EpisodeRating episodeRating)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(EpisodeRating)}: [{episodeRating}].");

            abContext.Update(episodeRating);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(EpisodeRating.Id)}: [{episodeRating.Id}].");
            return episodeRating;
        }
    }
}
