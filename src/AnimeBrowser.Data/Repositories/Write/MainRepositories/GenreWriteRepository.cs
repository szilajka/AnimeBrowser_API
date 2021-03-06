﻿using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Write.MainInterfaces;
using Serilog;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Write.MainRepositories
{
    public class GenreWriteRepository : IGenreWrite
    {
        private readonly AnimeBrowserContext abContext;
        private readonly ILogger logger = Log.ForContext<GenreWriteRepository>();

        public GenreWriteRepository(AnimeBrowserContext context)
        {
            abContext = context;
        }

        public async Task<Genre> CreateGenre(Genre genre)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(Genre)}: [{genre}].");

            await abContext.AddAsync(genre);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(Genre)}.{nameof(Genre.Id)}: [{genre.Id}].");
            return genre;
        }

        public async Task<Genre> UpdateGenre(Genre genre)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(Genre)}: [{genre}].");

            abContext.Update(genre);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(Genre)}.{nameof(Genre.Id)}: [{genre.Id}].");
            return genre;
        }

        public async Task DeleteGenre(Genre genre)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(Genre)}: [{genre}].");

            abContext.Genres.Remove(genre);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");
        }
    }
}
