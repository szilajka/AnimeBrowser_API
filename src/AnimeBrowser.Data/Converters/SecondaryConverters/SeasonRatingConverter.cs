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
    public static class SeasonRatingConverter
    {
        #region RequestModel
        public static SeasonRating ToSeasonRating(this SeasonRatingCreationRequestModel requestModel)
        {
            var seasonRating = new SeasonRating
            {
                Rating = requestModel.Rating,
                Message = requestModel.Message?.Trim(),
                SeasonId = requestModel.SeasonId,
                UserId = requestModel.UserId
            };
            return seasonRating;
        }

        public static SeasonRating ToSeasonRating(this SeasonRatingEditingRequestModel requestModel)
        {
            var seasonRating = new SeasonRating
            {
                Id = requestModel.Id,
                Rating = requestModel.Rating,
                Message = requestModel.Message?.Trim(),
                SeasonId = requestModel.SeasonId,
                UserId = requestModel.UserId
            };
            return seasonRating;
        }


        #endregion RequestModel

        #region ResponseModel
        public static SeasonRatingEditingResponseModel ToEditingResponseModel(this SeasonRating seasonRating)
        {
            var responseModel = new SeasonRatingEditingResponseModel(id: seasonRating.Id, rating: seasonRating.Rating, seasonId: seasonRating.SeasonId, userId: seasonRating.UserId, message: seasonRating.Message);
            return responseModel;
        }

        public static SeasonRatingCreationResponseModel ToCreationResponseModel(this SeasonRating seasonRating)
        {
            var responseModel = new SeasonRatingCreationResponseModel(id: seasonRating.Id, rating: seasonRating.Rating, seasonId: seasonRating.SeasonId, userId: seasonRating.UserId, message: seasonRating.Message);
            return responseModel;
        }

        #endregion ResponseModel
    }
}
