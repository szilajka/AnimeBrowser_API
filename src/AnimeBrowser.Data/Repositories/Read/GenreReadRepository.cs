using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read;
using Serilog;
using System;
using System.Linq;
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

        public async Task<Genre?> GetGenreById(long genreId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(genreId)}: [{genreId}].");

            var genre = await abContext.Genres.FindAsync(genreId);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(genre)}?.{nameof(genre.Id)}: [{genre?.Id}].");
            return genre;
        }

        public bool IsExistWithSameName(string genreName)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(genreName)}: [{genreName}].");

            var result = abContext.Genres.ToList().Any(g => g.GenreName.Equals(genreName, StringComparison.OrdinalIgnoreCase));

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(result)}: [{result}].");
            return result;
        }
    }
}
