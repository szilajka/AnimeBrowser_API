using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
using AnimeBrowser.Common.Models.ResponseModels.SecondaryModels;
using AnimeBrowser.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Converters.SecondaryConverters
{
    public static class SeasonNameConverter
    {
        #region RequestModel
        public static SeasonName ToSeasonName(this SeasonNameCreationRequestModel requestModel)
        {
            var seasonName = new SeasonName
            {
                Title = requestModel.Title?.Trim(),
                SeasonId = requestModel.SeasonId
            };
            return seasonName;
        }

        //public static SeasonName ToSeasonName(this SeasonNameEditingRequestModel requestModel)
        //{
        //    var seasonName = new SeasonName
        //    {
        //        Id = requestModel.Id,
        //        Title = requestModel.Title?.Trim(),
        //        SeasonId = requestModel.SeasonId
        //    };
        //    return seasonName;
        //}


        #endregion RequestModel

        #region ResponseModel
        //public static SeasonNameEditingResponseModel ToEditingResponseModel(this SeasonName seasonName)
        //{
        //    var responseModel = new SeasonNameEditingResponseModel(id: seasonName.Id, title: seasonName.Title, seasonId: seasonName.SeasonId);
        //    return responseModel;
        //}

        public static SeasonNameCreationResponseModel ToCreationResponseModel(this SeasonName seasonName)
        {
            var responseModel = new SeasonNameCreationResponseModel(id: seasonName.Id, title: seasonName.Title, seasonId: seasonName.SeasonId);
            return responseModel;
        }

        #endregion ResponseModel
    }
}
