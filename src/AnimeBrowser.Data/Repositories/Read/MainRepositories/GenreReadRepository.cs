using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.MainInterfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Read.MainRepositories
{
    public class GenreReadRepository : IGenreRead
    {
        private readonly AnimeBrowserContext abContext;
        private readonly ILogger logger = Log.ForContext<GenreReadRepository>();

        public GenreReadRepository(AnimeBrowserContext context)
        {
            abContext = context;
        }

        public async Task<Genre?> GetGenreById(long genreId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(genreId)}: [{genreId}].");

            var genre = await abContext.Genres.FindAsync(genreId);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(genre)}?.{nameof(genre.Id)}: [{genre?.Id}].");
            return genre;
        }

        public IList<Genre> GetGenresByIds(IEnumerable<long> genreIds)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(genreIds)}: [{string.Join(", ", genreIds)}].");

            var foundGenres = abContext.Genres.ToList().Where(g => genreIds.Contains(g.Id)).ToList();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(foundGenres)}.Count: [{foundGenres.Count}].");
            return foundGenres;
        }

        public bool IsExistWithSameName(string genreName)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(genreName)}: [{genreName}].");

            var result = abContext.Genres.ToList().Any(g => g.GenreName.Equals(genreName, StringComparison.OrdinalIgnoreCase));

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(result)}: [{result}].");
            return result;
        }

        public bool IsExistWithSameName(long genreId, string genreName)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(genreId)}: [{genreId}], {nameof(genreName)}: [{genreName}].");

            var result = abContext.Genres.ToList().Any(g => g.Id != genreId && g.GenreName.Equals(genreName, StringComparison.OrdinalIgnoreCase));

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(result)}: [{result}].");
            return result;
        }
    }
}
