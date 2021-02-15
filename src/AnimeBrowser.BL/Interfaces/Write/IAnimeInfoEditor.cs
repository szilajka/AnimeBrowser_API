using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Common.Models.ResponseModels;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write
{
    public interface IAnimeInfoEditor
    {
        Task<AnimeInfoCreationResponseModel> EditAnimeInfo(long id, AnimeInfoCreationRequestModel requestModel);
    }
}
