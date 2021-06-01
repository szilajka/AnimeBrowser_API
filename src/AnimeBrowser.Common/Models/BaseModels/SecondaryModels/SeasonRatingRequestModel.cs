using AnimeBrowser.Common.Attributes;

namespace AnimeBrowser.Common.Models.BaseModels.SecondaryModels
{
    [ToJsonString]
    public partial class SeasonRatingRequestModel
    {
        public SeasonRatingRequestModel(int rating, long seasonId, string userId, string message = "")
        {
            this.Rating = rating;
            this.Message = message;
            this.SeasonId = seasonId;
            this.UserId = userId;
        }

        public int Rating { get; set; }
        public string Message { get; set; }
        public long SeasonId { get; set; }
        public string UserId { get; set; }
    }
}
