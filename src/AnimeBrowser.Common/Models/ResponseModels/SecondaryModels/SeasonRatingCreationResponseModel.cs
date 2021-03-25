using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.ResponseModels.SecondaryModels
{
    public class SeasonRatingCreationResponseModel : SeasonRatingResponseModel
    {
        public SeasonRatingCreationResponseModel(long id, int rating, long seasonId, string userId, bool isAnimeInfoActive, bool isSeasonActive, string message = "")
            : base(id: id, rating: rating, seasonId: seasonId, userId: userId, message: message,
                  isAnimeInfoActive: isAnimeInfoActive, isSeasonActive: isSeasonActive)
        {
        }
    }
}
