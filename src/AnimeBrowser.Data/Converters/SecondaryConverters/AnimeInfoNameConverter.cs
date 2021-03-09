using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
using AnimeBrowser.Common.Models.ResponseModels.SecondaryModels;
using AnimeBrowser.Data.Entities;

namespace AnimeBrowser.Data.Converters.SecondaryConverters
{
    public static class AnimeInfoNameConverter
    {
        #region RequestModel
        public static AnimeInfoName ToAnimeInfoName(this AnimeInfoNameCreationRequestModel requestModel)
        {
            var animeInfoName = new AnimeInfoName
            {
                Title = requestModel.Title?.Trim(),
                AnimeInfoId = requestModel.AnimeInfoId
            };
            return animeInfoName;
        }

        public static AnimeInfoName ToAnimeInfoName(this AnimeInfoNameEditingRequestModel requestModel)
        {
            var animeInfoName = new AnimeInfoName
            {
                Id = requestModel.Id,
                Title = requestModel.Title?.Trim(),
                AnimeInfoId = requestModel.AnimeInfoId
            };
            return animeInfoName;
        }


        #endregion RequestModel

        #region ResponseModel
        public static AnimeInfoNameEditingResponseModel ToEditingResponseModel(this AnimeInfoName animeInfoName)
        {
            var responseModel = new AnimeInfoNameEditingResponseModel(id: animeInfoName.Id, title: animeInfoName.Title, animeInfoId: animeInfoName.AnimeInfoId);
            return responseModel;
        }

        public static AnimeInfoNameCreationResponseModel ToCreationResponseModel(this AnimeInfoName animeInfoName)
        {
            var responseModel = new AnimeInfoNameCreationResponseModel(id: animeInfoName.Id, title: animeInfoName.Title, animeInfoId: animeInfoName.AnimeInfoId);
            return responseModel;
        }


        #endregion ResponseModel
    }
}
