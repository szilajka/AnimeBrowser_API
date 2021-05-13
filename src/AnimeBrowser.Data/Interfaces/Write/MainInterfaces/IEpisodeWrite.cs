using AnimeBrowser.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Write.MainInterfaces
{
    public interface IEpisodeWrite
    {
        Task<Episode> CreateEpisode(Episode episode);
        Task<Episode> UpdateEpisode(Episode episode);
        Task DeleteEpisode(Episode episode, IEnumerable<EpisodeRating>? episodeRatings = null);

        Task UpdateEpisodeAndRatings(Episode episode, IEnumerable<EpisodeRating>? episodeRatings = null);
    }
}
