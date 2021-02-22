using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Common.Models.ResponseModels;
using AnimeBrowser.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Converters
{
    public static class SeasonConverter
    {
        #region RequestModel

        public static Season ToSeason(this SeasonCreationRequestModel requestModel)
        {
            var season = new Season
            {
                SeasonNumber = requestModel.SeasonNumber,
                Title = requestModel.Title.Trim(),
                Description = requestModel.Description.Trim(),
                StartDate = requestModel.StartDate,
                EndDate = requestModel.EndDate,
                AirStatus = (int)requestModel.AirStatus,
                NumberOfEpisodes = requestModel.NumberOfEpisodes,
                CoverCarousel = requestModel.CoverCarousel,
                Cover = requestModel.Cover,
                AnimeInfoId = requestModel.AnimeInfoId
            };

            return season;
        }

        //public static Season ToSeason(this SeasonEditingRequestModel requestModel)
        //{
        //    var season = new Season
        //    {
        //        Id = requestModel.Id,
        //        SeasonName = requestModel.SeasonName,
        //        Description = requestModel.Description
        //    };

        //    return season;
        //}

        #endregion RequestModel

        #region ResponseModel

        public static SeasonCreationResponseModel ToCreationResponseModel(this Season season)
        {
            var responseModel = new SeasonCreationResponseModel(id: season.Id, seasonNumber: season.SeasonNumber, title: season.Title, description: season.Description,
                startDate: season.StartDate, endDate: season.EndDate, airStatus: season.AirStatus, numberOfEpisodes: season.NumberOfEpisodes,
                coverCarousel: season.CoverCarousel, cover: season.Cover, animeInfoId: season.AnimeInfoId);
            return responseModel;
        }

        //public static SeasonEditingResponseModel ToEditingResponseModel(this Season season)
        //{
        //    var responseModel = new SeasonEditingResponseModel(id: season.Id, seasonName: season.SeasonName, description: season.Description);
        //    return responseModel;
        //}

        #endregion ResponseModel
    }
}
