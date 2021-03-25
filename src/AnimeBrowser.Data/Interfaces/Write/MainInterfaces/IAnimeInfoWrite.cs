using AnimeBrowser.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Write.MainInterfaces
{
    public interface IAnimeInfoWrite
    {
        Task<AnimeInfo> CreateAnimeInfo(AnimeInfo animeInfo);
        Task<AnimeInfo> UpdateAnimeInfo(AnimeInfo animeInfo);
        Task DeleteAnimeInfo(AnimeInfo animeInfo, IEnumerable<Season>? seasons = null, IEnumerable<Episode>? episodes = null,
            IEnumerable<SeasonRating>? seasonRatings = null, IEnumerable<EpisodeRating>? episodeRatings = null);
    }
}
