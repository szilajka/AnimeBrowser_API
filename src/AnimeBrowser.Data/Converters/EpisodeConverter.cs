using AnimeBrowser.Common.Models.Enums;
using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Common.Models.ResponseModels;
using AnimeBrowser.Data.Entities;

namespace AnimeBrowser.Data.Converters
{
    public static class EpisodeConverter
    {
        #region RequestModel
        public static Episode ToEpisode(this EpisodeCreationRequestModel requestModel)
        {
            var episode = new Episode
            {
                EpisodeNumber = requestModel.EpisodeNumber,
                AirStatus = (int)requestModel.AirStatus,
                Title = requestModel.Title?.Trim(),
                Description = requestModel.Description?.Trim(),
                AirDate = requestModel.AirDate,
                Cover = requestModel.Cover,
                SeasonId = requestModel.SeasonId,
                AnimeInfoId = requestModel.AnimeInfoId

            };
            return episode;
        }

        //public static Episode ToEpisode(this EpisodeEditingRequestModel requestModel)
        //{
        //    var episode = new Episode
        //    {
        //        Id = requestModel.Id,
        //        EpisodeNumber = requestModel.EpisodeNumber,
        //        AirStatus = (int)requestModel.AirStatus,
        //        Title = requestModel.Title?.Trim(),
        //        Description = requestModel.Description?.Trim(),
        //        AirDate = requestModel.AirDate,
        //        Cover = requestModel.Cover,
        //        SeasonId = requestModel.SeasonId,
        //        AnimeInfoId = requestModel.AnimeInfoId

        //    };
        //    return episode;
        //}


        #endregion RequestModel

        #region ResponseModel
        //public static EpisodeEditingResponseModel ToEditingResponseModel(this Episode episode)
        //{
        //    var responseModel = new EpisodeEditingResponseModel(id: episode.Id, episodeNumber: episode.EpisodeNumber, airStatus: (AirStatusEnum)episode.AirStatus,
        //        title: episode.Title, description: episode.Description, airDate: episode.AirDate,
        //        cover: episode.Cover, seasonId: episode.SeasonId, animeInfoId: episode.AnimeInfoId);
        //    return responseModel;
        //}

        public static EpisodeCreationResponseModel ToCreationResponseModel(this Episode episode)
        {
            var responseModel = new EpisodeCreationResponseModel(id: episode.Id, episodeNumber: episode.EpisodeNumber, airStatus: (AirStatusEnum)episode.AirStatus,
                title: episode.Title, description: episode.Description, airDate: episode.AirDate,
                cover: episode.Cover, seasonId: episode.SeasonId, animeInfoId: episode.AnimeInfoId);
            return responseModel;
        }


        #endregion ResponseModel
    }
}
