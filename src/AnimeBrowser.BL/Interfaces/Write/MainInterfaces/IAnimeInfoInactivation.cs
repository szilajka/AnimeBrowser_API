using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.MainInterfaces
{
    public interface IAnimeInfoInactivation
    {
        Task<AnimeInfo> Inactivate(long animeInfoId);
    }
}
