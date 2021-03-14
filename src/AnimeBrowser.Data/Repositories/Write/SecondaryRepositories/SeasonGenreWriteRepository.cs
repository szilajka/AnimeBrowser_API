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
    public class SeasonGenreWriteRepository : ISeasonGenreWrite
    {
        private readonly ILogger logger = Log.ForContext<SeasonGenreWriteRepository>();
        private readonly AnimeBrowserContext abContext;

        public SeasonGenreWriteRepository(AnimeBrowserContext abContext)
        {
            this.abContext = abContext;
        }

        public async Task<SeasonGenre> CreateSeasonGenre(SeasonGenre seasonGenre)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(SeasonGenre)}: [{seasonGenre}].");

            await abContext.AddAsync(seasonGenre);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(SeasonGenre.Id)}: [{seasonGenre.Id}].");
            return seasonGenre;
        }

        public async Task<IEnumerable<SeasonGenre>> CreateSeasonGenres(IEnumerable<SeasonGenre> seasonGenres)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(SeasonGenre)}s: [{string.Join(", ", seasonGenres)}].");

            await abContext.AddRangeAsync(seasonGenres);
            await abContext.SaveChangesAsync();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(SeasonGenre.Id)}s: [{string.Join(", ", seasonGenres.Select(sg => sg.Id))}].");
            return seasonGenres;
        }
    }
}
