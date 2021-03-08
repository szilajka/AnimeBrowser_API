using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.MainInterfaces
{
    public interface IAnimeInfoDelete
    {
        Task DeleteAnimeInfo(long id);
    }
}
