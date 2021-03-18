using AnimeBrowser.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Write.SecondaryInterfaces
{
    public interface ISeasonGenreWrite
    {
        Task<SeasonGenre> CreateSeasonGenre(SeasonGenre seasonGenre);
        Task<IEnumerable<SeasonGenre>> CreateSeasonGenres(IEnumerable<SeasonGenre> seasonGenres);

        Task DeleteSeasonGenres(IEnumerable<SeasonGenre> seasonGenres);
    }
}
