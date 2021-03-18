using AnimeBrowser.Data.Entities;
using System.Collections.Generic;

namespace AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces
{
    public interface ISeasonGenreRead
    {
        SeasonGenre? GetSeasonGenreBySeasonAndGenreId(long seasonId, long genreId);
        IList<SeasonGenre> GetSeasonGenreBySeasonAndGenreIds(IEnumerable<(long SeasonId, long GenreId)> seasonAndGenreIds);
        IList<SeasonGenre> GetSeasonGenresByIds(IEnumerable<long> seasonGenreIds);
    }
}
