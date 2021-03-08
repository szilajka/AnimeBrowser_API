using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.MainInterfaces
{
    public interface IGenreDelete
    {
        Task DeleteGenre(long genreId);
    }
}
