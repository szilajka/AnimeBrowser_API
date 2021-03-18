using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
using AnimeBrowser.Common.Models.ResponseModels.SecondaryModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces
{
    public interface ISeasonGenreCreation
    {
        Task<IEnumerable<SeasonGenreCreationResponseModel>> CreateSeasonGenres(IList<SeasonGenreCreationRequestModel> requestModel);
    }
}
