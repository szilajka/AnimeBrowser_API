using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.RequestModels.SecondaryModels
{
    public partial class SeasonRatingEditingRequestModel : SeasonRatingRequestModel
    {
        public SeasonRatingEditingRequestModel(long id, int rating, long seasonId, string userId, string message = "")
             : base(rating: rating, seasonId: seasonId, userId: userId, message: message)
        {
            this.Id = id;
        }

        public long Id { get; set; }
    }
}
