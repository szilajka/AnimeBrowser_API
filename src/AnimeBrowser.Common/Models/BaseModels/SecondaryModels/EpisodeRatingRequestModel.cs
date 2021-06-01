using AnimeBrowser.Common.Attributes;

namespace AnimeBrowser.Common.Models.BaseModels.SecondaryModels
{
    [ToJsonString]
    public partial class EpisodeRatingRequestModel
    {
        public EpisodeRatingRequestModel(int rating, long episodeId, string userId, string message = "")
        {
            this.Rating = rating;
            this.EpisodeId = episodeId;
            this.UserId = userId;
            this.Message = message;
        }

        public int Rating { get; set; }
        public string Message { get; set; }
        public long EpisodeId { get; set; }
        public string UserId { get; set; }
    }
}
