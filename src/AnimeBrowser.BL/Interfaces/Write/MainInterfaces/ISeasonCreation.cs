﻿using AnimeBrowser.Common.Models.RequestModels.MainModels;
using AnimeBrowser.Common.Models.ResponseModels.MainModels;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.MainInterfaces
{
    public interface ISeasonCreation
    {
        Task<SeasonCreationResponseModel> CreateSeason(SeasonCreationRequestModel seasonRequestModel);
    }
}
