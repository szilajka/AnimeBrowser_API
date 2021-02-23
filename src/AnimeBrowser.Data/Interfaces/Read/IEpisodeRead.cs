using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Read
{
    public interface IEpisodeRead
    {
        Task<Episode?> GetEpisodeById(long episodeId);
        Task<bool> IsSeasonAndAnimeInfoExistsAndReferences(long seasonId, long animeInfoId);
    }
}
