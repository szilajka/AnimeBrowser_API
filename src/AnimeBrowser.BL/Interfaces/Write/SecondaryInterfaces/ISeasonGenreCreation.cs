using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
using AnimeBrowser.Common.Models.ResponseModels.SecondaryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces
{
    public interface ISeasonGenreCreation
    {
        Task<IEnumerable<SeasonGenreCreationResponseModel>> CreateSeasonGenres(IList<SeasonGenreCreationRequestModel> requestModel);
    }
}
