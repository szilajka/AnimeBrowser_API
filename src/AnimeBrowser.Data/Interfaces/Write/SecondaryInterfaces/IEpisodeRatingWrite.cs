using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Write.SecondaryInterfaces
{
    public interface IEpisodeRatingWrite
    {
        Task<EpisodeRating> CreateEpisodeRating(EpisodeRating episodeRating);
        Task<EpisodeRating> UpdateEpisodeRating(EpisodeRating episodeRating);
    }
}
