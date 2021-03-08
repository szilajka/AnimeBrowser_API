using AnimeBrowser.Common.Models.BaseModels.MainModels;
using AnimeBrowser.Common.Models.RequestModels.MainModels;
using AnimeBrowser.Common.Models.ResponseModels.MainModels;
using AnimeBrowser.Data.Entities;

namespace AnimeBrowser.Data.Converters.MainConverters
{
    public static class AnimeInfoConverter
    {

        #region RequestModel
        public static AnimeInfo ToAnimeInfo(this AnimeInfoCreationRequestModel requestModel)
        {
            var animeInfo = new AnimeInfo
            {
                Title = requestModel.Title?.Trim(),
                Description = requestModel.Description?.Trim(),
                IsNsfw = requestModel.IsNsfw
            };
            return animeInfo;
        }

        public static AnimeInfo ToAnimeInfo(this AnimeInfoEditingRequestModel requestModel)
        {
            var animeInfo = new AnimeInfo
            {
                Id = requestModel.Id,
                Title = requestModel.Title?.Trim(),
                Description = requestModel.Description?.Trim(),
                IsNsfw = requestModel.IsNsfw
            };
            return animeInfo;
        }


        #endregion RequestModel

        #region ResponseModel
        public static AnimeInfoResponseModel ToBasicResponseModel(this AnimeInfo animeInfo)
        {
            var responseModel = new AnimeInfoResponseModel(id: animeInfo.Id, title: animeInfo.Title, description: animeInfo.Description, isNsfw: animeInfo.IsNsfw);
            return responseModel;
        }

        public static AnimeInfoEditingResponseModel ToEditingResponseModel(this AnimeInfo animeInfo)
        {
            var responseModel = new AnimeInfoEditingResponseModel(id: animeInfo.Id, title: animeInfo.Title, description: animeInfo.Description, isNsfw: animeInfo.IsNsfw);
            return responseModel;
        }

        public static AnimeInfoCreationResponseModel ToCreationResponseModel(this AnimeInfo animeInfo)
        {
            var responseModel = new AnimeInfoCreationResponseModel(id: animeInfo.Id, title: animeInfo.Title, description: animeInfo.Description, isNsfw: animeInfo.IsNsfw);
            return responseModel;
        }


        #endregion ResponseModel
    }
}
