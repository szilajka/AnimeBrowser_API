using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write
{
    public interface IAnimeInfoDelete
    {
        Task DeleteAnimeInfo(long id);
    }
}
