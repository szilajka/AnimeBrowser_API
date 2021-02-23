using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Read
{
    public interface IAnimeInfoRead
    {
        Task<AnimeInfo?> GetAnimeInfoById(long id);
    }
}
