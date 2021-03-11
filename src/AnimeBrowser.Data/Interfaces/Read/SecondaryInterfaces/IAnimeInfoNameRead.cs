using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces
{
    public interface IAnimeInfoNameRead
    {
        Task<AnimeInfoName?> GetAnimeInfoNameById(long id);
        bool IsExistingWithSameTitle(long id, string title, long animeInfoId);
        bool IsExistingWithSameTitle(string title, long animeInfoId);
    }
}
