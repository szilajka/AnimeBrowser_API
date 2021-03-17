using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.ResponseModels.SecondaryModels
{
    public class SeasonRatingEditingResponseModel : SeasonRatingResponseModel
    {
        public SeasonRatingEditingResponseModel(long id, int rating, long seasonId, string userId, string message = "")
            : base(id: id, rating: rating, seasonId: seasonId, userId: userId, message: message)
        {
        }
    }
}
