using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Write.SecondaryInterfaces
{
    public interface IAnimeInfoNameWrite
    {
        Task<AnimeInfoName> CreateAnimeInfoName(AnimeInfoName animeInfoName);
        Task<AnimeInfoName> UpdateAnimeInfoName(AnimeInfoName animeInfoName);
        Task DeleteAnimeInfoName(AnimeInfoName animeInfoName);
    }
}
