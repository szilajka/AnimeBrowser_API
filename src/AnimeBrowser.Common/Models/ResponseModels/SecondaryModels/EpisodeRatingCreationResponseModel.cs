using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.ResponseModels.SecondaryModels
{
    public class EpisodeRatingCreationResponseModel : EpisodeRatingResponseModel
    {
        public EpisodeRatingCreationResponseModel(long id, int rating, long episodeId, string userId, string message = "")
            : base(id: id, rating: rating, episodeId: episodeId, userId: userId, message: message)
        {
        }
    }
}
