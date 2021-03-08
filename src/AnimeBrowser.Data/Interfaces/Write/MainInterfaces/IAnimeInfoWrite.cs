using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Write.MainInterfaces
{
    public interface IAnimeInfoWrite
    {
        Task<AnimeInfo> CreateAnimeInfo(AnimeInfo animeInfo);
        Task<AnimeInfo> UpdateAnimeInfo(AnimeInfo animeInfo);
        Task DeleteAnimeInfo(AnimeInfo animeInfo);
    }
}
