﻿using AnimeBrowser.Common.Models.RequestModels.MainModels;
using AnimeBrowser.Common.Models.ResponseModels.MainModels;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.MainInterfaces
{
    public interface ISeasonEditing
    {
        Task<SeasonEditingResponseModel> EditSeason(long id, SeasonEditingRequestModel seasonRequestModel);
    }
}
