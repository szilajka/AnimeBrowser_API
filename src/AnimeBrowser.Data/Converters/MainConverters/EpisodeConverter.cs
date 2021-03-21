using AnimeBrowser.Common.Models.Enums;
using AnimeBrowser.Common.Models.RequestModels.MainModels;
using AnimeBrowser.Common.Models.ResponseModels.MainModels;
using AnimeBrowser.Data.Entities;

namespace AnimeBrowser.Data.Converters.MainConverters
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

        public static Episode ToEpisode(this EpisodeEditingRequestModel requestModel)
        {
            var episode = new Episode
            {
                Id = requestModel.Id,
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


        #endregion RequestModel

        #region ResponseModel
        public static EpisodeEditingResponseModel ToEditingResponseModel(this Episode episode)
        {
            var responseModel = new EpisodeEditingResponseModel(id: episode.Id, episodeNumber: episode.EpisodeNumber, airStatus: (AirStatuses)episode.AirStatus,
                title: episode.Title, description: episode.Description, airDate: episode.AirDate,
                cover: episode.Cover, seasonId: episode.SeasonId, animeInfoId: episode.AnimeInfoId);
            return responseModel;
        }

        public static EpisodeCreationResponseModel ToCreationResponseModel(this Episode episode)
        {
            var responseModel = new EpisodeCreationResponseModel(id: episode.Id, episodeNumber: episode.EpisodeNumber, airStatus: (AirStatuses)episode.AirStatus,
                title: episode.Title, description: episode.Description, airDate: episode.AirDate,
                cover: episode.Cover, seasonId: episode.SeasonId, animeInfoId: episode.AnimeInfoId);
            return responseModel;
        }

        #endregion ResponseModel

        public static void UpdateEpisodeWithOtherEpisode(this Episode oldValuesEpisode, Episode newerEpisode)
        {
            oldValuesEpisode.EpisodeNumber = newerEpisode.EpisodeNumber;
            oldValuesEpisode.AirStatus = newerEpisode.AirStatus;
            oldValuesEpisode.Title = newerEpisode.Title;
            oldValuesEpisode.Description = newerEpisode.Description;
            oldValuesEpisode.AirDate = newerEpisode.AirDate;
            oldValuesEpisode.Cover = newerEpisode.Cover;
            oldValuesEpisode.AnimeInfoId = newerEpisode.AnimeInfoId;
            oldValuesEpisode.SeasonId = newerEpisode.SeasonId;
        }
    }
}
