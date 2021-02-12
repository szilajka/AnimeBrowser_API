using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Common.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write
{
    public interface IAnimeInfoCreation
    {
        Task<AnimeInfoCreationResponseModel> CreateAnimeInfo(AnimeInfoCreationRequestModel animeInfo);
    }
}
