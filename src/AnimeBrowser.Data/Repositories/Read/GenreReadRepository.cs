using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Read
{
    public class GenreReadRepository : IGenreRead
    {
        private readonly AnimeBrowserContext abContext;
        private readonly ILogger logger = Log.ForContext<GenreReadRepository>();

        public GenreReadRepository(AnimeBrowserContext context)
        {
            this.abContext = context;
        }

        public async Task<Genre> GetGenreById(long genreId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. genreId: [{genreId}].");

            var genre = await abContext.Genres.FindAsync(genreId);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. genre?.Id: [{genre?.Id}].");
            return genre;
        }
    }
}
