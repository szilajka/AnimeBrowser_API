using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
using AnimeBrowser.Common.Models.ResponseModels.SecondaryModels;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces
{
    public interface ISeasonNameCreation
    {
        Task<SeasonNameCreationResponseModel> CreateSeasonName(SeasonNameCreationRequestModel seasonNameRequestModel);
    }
}
