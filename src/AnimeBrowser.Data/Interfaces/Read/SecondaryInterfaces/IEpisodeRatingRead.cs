using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces
{
    public interface IEpisodeRatingRead
    {
        EpisodeRating? GetEpisodeRatingByEpisodeAndUserId(long episodeId, string userId);
    }
}
