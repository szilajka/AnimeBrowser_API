using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.MainInterfaces
{
    public interface IAnimeInfoActivation
    {
        Task<AnimeInfo> Activate(long animeInfoId);
    }
}
