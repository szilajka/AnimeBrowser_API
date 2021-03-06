﻿using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
using AnimeBrowser.Common.Models.ResponseModels.SecondaryModels;
using AnimeBrowser.Data.Entities;

namespace AnimeBrowser.Data.Converters.SecondaryConverters
{
    public static class EpisodeRatingConverter
    {
        #region RequestModel
        public static EpisodeRating ToEpisodeRating(this EpisodeRatingCreationRequestModel requestModel)
        {
            var episodeRating = new EpisodeRating
            {
                Rating = requestModel.Rating,
                Message = requestModel.Message?.Trim(),
                EpisodeId = requestModel.EpisodeId,
                UserId = requestModel.UserId
            };
            return episodeRating;
        }

        public static EpisodeRating ToEpisodeRating(this EpisodeRatingEditingRequestModel requestModel)
        {
            var episodeRating = new EpisodeRating
            {
                Id = requestModel.Id,
                Rating = requestModel.Rating,
                Message = requestModel.Message?.Trim(),
                EpisodeId = requestModel.EpisodeId,
                UserId = requestModel.UserId
            };
            return episodeRating;
        }


        #endregion RequestModel

        #region ResponseModel
        public static EpisodeRatingEditingResponseModel ToEditingResponseModel(this EpisodeRating episodeRating)
        {
            var responseModel = new EpisodeRatingEditingResponseModel(id: episodeRating.Id, rating: episodeRating.Rating, episodeId: episodeRating.EpisodeId, userId: episodeRating.UserId, message: episodeRating.Message,
                isAnimeInfoActive: episodeRating.IsAnimeInfoActive.Value, isSeasonActive: episodeRating.IsSeasonActive.Value, isEpisodeActive: episodeRating.IsEpisodeActive.Value);
            return responseModel;
        }

        public static EpisodeRatingCreationResponseModel ToCreationResponseModel(this EpisodeRating episodeRating)
        {
            var responseModel = new EpisodeRatingCreationResponseModel(id: episodeRating.Id, rating: episodeRating.Rating, episodeId: episodeRating.EpisodeId, userId: episodeRating.UserId, message: episodeRating.Message,
                isAnimeInfoActive: episodeRating.IsAnimeInfoActive.Value, isSeasonActive: episodeRating.IsSeasonActive.Value, isEpisodeActive: episodeRating.IsEpisodeActive.Value);
            return responseModel;
        }

        #endregion ResponseModel
    }
}
