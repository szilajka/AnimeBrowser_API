using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.RequestModels.SecondaryModels
{
    public class EpisodeRatingCreationRequestModel : EpisodeRatingRequestModel
    {
        public EpisodeRatingCreationRequestModel(int rating, long episodeId, string userId, string message = "")
            : base(rating: rating, episodeId: episodeId, userId: userId, message: message)
        {
        }
    }
}
