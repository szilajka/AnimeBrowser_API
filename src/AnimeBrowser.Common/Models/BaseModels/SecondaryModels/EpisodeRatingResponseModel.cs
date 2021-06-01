using AnimeBrowser.Common.Attributes;

namespace AnimeBrowser.Common.Models.BaseModels.SecondaryModels
{
    [ToJsonString]
    public partial class EpisodeRatingResponseModel
    {
        public EpisodeRatingResponseModel(long id, int rating, long episodeId, string userId,
            bool isAnimeInfoActive, bool isSeasonActive, bool isEpisodeActive, string message = "")
        {
            this.Id = id;
            this.Rating = rating;
            this.EpisodeId = episodeId;
            this.UserId = userId;
            this.Message = message;
            this.IsAnimeInfoActive = isAnimeInfoActive;
            this.IsSeasonActive = isSeasonActive;
            this.IsEpisodeActive = isEpisodeActive;
        }

        public long Id { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
        public long EpisodeId { get; set; }
        public string UserId { get; set; }
        public bool IsAnimeInfoActive { get; set; }
        public bool IsSeasonActive { get; set; }
        public bool IsEpisodeActive { get; set; }
    }
}
