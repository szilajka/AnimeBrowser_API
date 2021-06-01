using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.RequestModels.SecondaryModels
{
    public partial class EpisodeRatingEditingRequestModel : EpisodeRatingRequestModel
    {
        public EpisodeRatingEditingRequestModel(long id, int rating, long episodeId, string userId, string message = "")
            : base(rating: rating, episodeId: episodeId, userId: userId, message: message)
        {
            this.Id = id;
        }

        public long Id { get; set; }
    }
}
