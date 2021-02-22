using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Common.Models.ResponseModels;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write
{
    public interface ISeasonCreation
    {
        Task<SeasonCreationResponseModel> CreateSeason(SeasonCreationRequestModel seasonRequestModel);
    }
}
