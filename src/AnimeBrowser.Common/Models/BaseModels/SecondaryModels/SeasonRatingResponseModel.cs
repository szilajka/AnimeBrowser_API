using AnimeBrowser.Common.Attributes;

namespace AnimeBrowser.Common.Models.BaseModels.SecondaryModels
{
    [ToJsonString]
    public partial class SeasonRatingResponseModel
    {
        public SeasonRatingResponseModel(long id, int rating, long seasonId, string userId, bool isAnimeInfoActive, bool isSeasonActive, string message = "")
        {
            this.Id = id;
            this.Rating = rating;
            this.Message = message;
            this.SeasonId = seasonId;
            this.UserId = userId;
            this.IsAnimeInfoActive = isAnimeInfoActive;
            this.IsSeasonActive = isSeasonActive;
        }

        public long Id { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
        public long SeasonId { get; set; }
        public string UserId { get; set; }
        public bool IsAnimeInfoActive { get; set; }
        public bool IsSeasonActive { get; set; }
    }
}
