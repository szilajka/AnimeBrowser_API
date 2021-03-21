using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
using AnimeBrowser.Common.Models.ResponseModels.SecondaryModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces
{
    public interface IEpisodeRatingEditing
    {
        Task<EpisodeRatingEditingResponseModel> EditEpisodeRating(long id, EpisodeRatingEditingRequestModel episodeRatingRequestModel, IEnumerable<Claim>? claims);
    }
}
