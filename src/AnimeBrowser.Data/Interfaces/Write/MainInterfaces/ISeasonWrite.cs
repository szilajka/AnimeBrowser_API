using AnimeBrowser.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Write.MainInterfaces
{
    public interface ISeasonWrite
    {
        Task<Season> CreateSeason(Season season);
        Task<Season> UpdateSeason(Season season);
        Task DeleteSeason(Season season, IEnumerable<Episode>? episodes = null, IEnumerable<SeasonRating>? seasonRatings = null, IEnumerable<EpisodeRating>? episodeRatings = null);
        void UpdateSeasonActiveStatus(Season season, IEnumerable<SeasonRating>? seasonRatings, IEnumerable<Episode>? episodes, IEnumerable<EpisodeRating>? episodeRatings);
    }
}
