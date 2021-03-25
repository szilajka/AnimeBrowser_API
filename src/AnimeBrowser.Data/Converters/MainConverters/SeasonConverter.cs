using AnimeBrowser.Common.Models.RequestModels.MainModels;
using AnimeBrowser.Common.Models.ResponseModels.MainModels;
using AnimeBrowser.Data.Entities;

namespace AnimeBrowser.Data.Converters.MainConverters
{
    public static class SeasonConverter
    {
        #region RequestModel

        public static Season ToSeason(this SeasonCreationRequestModel requestModel)
        {
            var season = new Season
            {
                SeasonNumber = requestModel.SeasonNumber,
                Title = requestModel.Title?.Trim(),
                Description = requestModel.Description?.Trim(),
                StartDate = requestModel.StartDate,
                EndDate = requestModel.EndDate,
                AirStatus = (int)requestModel.AirStatus,
                NumberOfEpisodes = requestModel.NumberOfEpisodes,
                CoverCarousel = requestModel.CoverCarousel,
                Cover = requestModel.Cover,
                AnimeInfoId = requestModel.AnimeInfoId,
                IsActive = requestModel.IsActive
            };

            return season;
        }

        public static Season ToSeason(this SeasonEditingRequestModel requestModel)
        {
            var season = new Season
            {
                Id = requestModel.Id,
                SeasonNumber = requestModel.SeasonNumber,
                Title = requestModel.Title?.Trim(),
                Description = requestModel.Description?.Trim(),
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

        #endregion RequestModel

        #region ResponseModel

        public static SeasonCreationResponseModel ToCreationResponseModel(this Season season)
        {
            var responseModel = new SeasonCreationResponseModel(id: season.Id, seasonNumber: season.SeasonNumber, title: season.Title, description: season.Description,
                startDate: season.StartDate, endDate: season.EndDate, airStatus: season.AirStatus, numberOfEpisodes: season.NumberOfEpisodes,
                coverCarousel: season.CoverCarousel, cover: season.Cover, animeInfoId: season.AnimeInfoId,
                isActive: season.IsActive.Value, isAnimeInfoActive: season.IsAnimeInfoActive.Value);
            return responseModel;
        }

        public static SeasonEditingResponseModel ToEditingResponseModel(this Season season)
        {
            var responseModel = new SeasonEditingResponseModel(id: season.Id, seasonNumber: season.SeasonNumber, title: season.Title, description: season.Description,
                startDate: season.StartDate, endDate: season.EndDate, airStatus: season.AirStatus, numberOfEpisodes: season.NumberOfEpisodes,
                coverCarousel: season.CoverCarousel, cover: season.Cover, animeInfoId: season.AnimeInfoId,
                isActive: season.IsActive.Value, isAnimeInfoActive: season.IsAnimeInfoActive.Value);
            return responseModel;
        }

        #endregion ResponseModel

        public static void UpdateSeasonWithOtherSeason(this Season oldValuesSeason, Season newerSeason)
        {
            oldValuesSeason.SeasonNumber = newerSeason.SeasonNumber;
            oldValuesSeason.Title = newerSeason.Title;
            oldValuesSeason.Description = newerSeason.Description;
            oldValuesSeason.StartDate = newerSeason.StartDate;
            oldValuesSeason.EndDate = newerSeason.EndDate;
            oldValuesSeason.AirStatus = newerSeason.AirStatus;
            oldValuesSeason.NumberOfEpisodes = newerSeason.NumberOfEpisodes;
            oldValuesSeason.CoverCarousel = newerSeason.CoverCarousel;
            oldValuesSeason.Cover = newerSeason.Cover;
            oldValuesSeason.AnimeInfoId = newerSeason.AnimeInfoId;
        }
    }
}
