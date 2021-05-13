using AnimeBrowser.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces
{
    public interface IEpisodeRatingRead
    {
        Task<EpisodeRating?> GetEpisodeRatingById(long id);
        EpisodeRating? GetEpisodeRatingByEpisodeAndUserId(long episodeId, string userId);
        IEnumerable<EpisodeRating>? GetEpisodeRatingsByEpisodeId(long episodeId);
        IEnumerable<EpisodeRating>? GetEpisodeRatingsByEpisodeIds(IEnumerable<long>? episodeIds);
    }
}
