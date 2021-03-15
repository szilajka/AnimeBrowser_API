using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Read.SecondaryRepositories
{
    public class SeasonGenreReadRepository : ISeasonGenreRead
    {
        private readonly ILogger logger = Log.ForContext<SeasonGenreReadRepository>();
        private readonly AnimeBrowserContext abContext;

        public SeasonGenreReadRepository(AnimeBrowserContext abContext)
        {
            this.abContext = abContext;
        }

        public SeasonGenre? GetSeasonGenreBySeasonAndGenreId(long seasonId, long genreId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonId)}: [{seasonId}], {nameof(genreId)}: [{genreId}].");

            var seasonGenre = abContext.SeasonGenres.ToList().SingleOrDefault(sg => sg.SeasonId == seasonId && sg.GenreId == genreId);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Found {nameof(seasonGenre)}.{nameof(seasonGenre.Id)}: [{seasonGenre?.Id}].");
            return seasonGenre;
        }

        public IList<SeasonGenre> GetSeasonGenreBySeasonAndGenreIds(IEnumerable<(long SeasonId, long GenreId)> seasonAndGenreIds)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonAndGenreIds)}: [{string.Join(", ", seasonAndGenreIds)}].");

            var allSeasonGenres = abContext.SeasonGenres.ToList();
            List<SeasonGenre> foundSeasonGenres = new();
            foreach (var sgIds in seasonAndGenreIds)
            {
                var fSG = allSeasonGenres.Where(sg => sg.SeasonId == sgIds.SeasonId && sg.GenreId == sgIds.GenreId);
                if (fSG?.Count() > 0)
                {
                    foundSeasonGenres.AddRange(fSG);
                }
            }

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Found {nameof(foundSeasonGenres)}.{nameof(foundSeasonGenres.Count)}: [{foundSeasonGenres.Count}].");
            return foundSeasonGenres;
        }

        public IList<SeasonGenre> GetSeasonGenresByIds(IEnumerable<long> seasonGenreIds)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonGenreIds)}: [{string.Join(", ", seasonGenreIds)}].");

            var seasonGenres = abContext.SeasonGenres.ToList().Where(sg => seasonGenreIds.Contains(sg.Id)).ToList();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Found {nameof(seasonGenres)}.{nameof(seasonGenres.Count)}: [{seasonGenres.Count}].");
            return seasonGenres;
        }
    }
}
