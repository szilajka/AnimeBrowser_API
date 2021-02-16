using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Write
{
    public interface IAnimeInfoWrite
    {
        Task<AnimeInfo> CreateAnimeInfo(AnimeInfo animeInfo);
        Task<AnimeInfo> UpdateAnimeInfo(AnimeInfo animeInfo);
    }
}
