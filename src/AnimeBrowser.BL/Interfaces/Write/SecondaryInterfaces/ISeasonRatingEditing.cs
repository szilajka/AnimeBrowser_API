using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
using AnimeBrowser.Common.Models.ResponseModels.SecondaryModels;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces
{
    public interface ISeasonRatingEditing
    {
        Task<SeasonRatingEditingResponseModel> EditSeasonRating(long id, SeasonRatingEditingRequestModel seasonRatingRequestModel);
    }
}
