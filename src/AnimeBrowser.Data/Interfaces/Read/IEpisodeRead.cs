using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Read
{
    public interface IEpisodeRead
    {
        Task<Episode?> GetEpisodeById(long episodeId);
        bool IsEpisodeWithEpisodeNumberExists(long seasonId, int episodeNumber);
        Task<bool> IsSeasonAndAnimeInfoExistsAndReferences(long seasonId, long animeInfoId);
    }
}
