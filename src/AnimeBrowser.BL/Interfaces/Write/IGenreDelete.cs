using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write
{
    public interface IGenreDelete
    {
        Task DeleteGenre(long genreId);
    }
}
