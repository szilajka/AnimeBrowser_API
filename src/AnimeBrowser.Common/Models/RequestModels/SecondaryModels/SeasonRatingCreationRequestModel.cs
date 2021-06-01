using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.RequestModels.SecondaryModels
{
    public partial class SeasonRatingCreationRequestModel : SeasonRatingRequestModel
    {
        public SeasonRatingCreationRequestModel(int rating, long seasonId, string userId, string message = "")
            : base(rating: rating, seasonId: seasonId, userId: userId, message: message)
        {
        }
    }
}
