using AnimeBrowser.Common.Attributes;
using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.ResponseModels.SecondaryModels
{
    [ToJsonString]
    public partial class SeasonRatingEditingResponseModel : SeasonRatingResponseModel
    {
        public SeasonRatingEditingResponseModel(long id, int rating, long seasonId, string userId, bool isAnimeInfoActive, bool isSeasonActive, string message = "")
            : base(id: id, rating: rating, seasonId: seasonId, userId: userId, message: message,
                  isAnimeInfoActive: isAnimeInfoActive, isSeasonActive: isSeasonActive)
        {
        }
    }
}
