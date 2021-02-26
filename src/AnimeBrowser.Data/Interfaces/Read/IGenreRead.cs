using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Read
{
    public interface IGenreRead
    {
        Task<Genre?> GetGenreById(long genreId);
        bool IsExistWithSameName(string genreName);
    }
}
