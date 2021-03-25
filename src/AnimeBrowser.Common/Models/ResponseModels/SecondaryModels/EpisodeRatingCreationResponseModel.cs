using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.ResponseModels.SecondaryModels
{
    public class EpisodeRatingCreationResponseModel : EpisodeRatingResponseModel
    {
        public EpisodeRatingCreationResponseModel(long id, int rating, long episodeId, string userId, bool isAnimeInfoActive, bool isSeasonActive, bool isEpisodeActive, string message = "")
            : base(id: id, rating: rating, episodeId: episodeId, userId: userId, message: message,
                  isAnimeInfoActive: isAnimeInfoActive, isSeasonActive: isSeasonActive, isEpisodeActive: isEpisodeActive)
        {
        }
    }
}
