using AnimeBrowser.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Read.MainInterfaces
{
    public interface IGenreRead
    {
        Task<Genre?> GetGenreById(long genreId);
        IList<Genre> GetGenresByIds(IEnumerable<long> genreIds);

        // Use this when you are creating a genre
        bool IsExistWithSameName(string genreName);
        // Use this when you are editing a genre (to filter out the one that wants to change its name)
        bool IsExistWithSameName(long genreId, string genreName);
    }
}
