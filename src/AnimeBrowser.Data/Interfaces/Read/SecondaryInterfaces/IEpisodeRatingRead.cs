using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces
{
    public interface IEpisodeRatingRead
    {
        Task<EpisodeRating?> GetEpisodeRatingById(long id);
        EpisodeRating? GetEpisodeRatingByEpisodeAndUserId(long episodeId, string userId);
    }
}
