using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Write
{
    public interface IGenreWrite
    {
        Task<Genre> CreateGenre(Genre genre);
        Task<Genre> UpdateGenre(Genre genre);
        Task DeleteGenre(Genre genre);
    }
}
