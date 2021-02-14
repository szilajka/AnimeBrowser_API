using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Common.Models.ResponseModels;
using AnimeBrowser.Data.Entities;

namespace AnimeBrowser.Data.Converters
{
    public static class AnimeInfoConverter
    {
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

        public static AnimeInfoCreationResponseModel ToCreationResponseModel(this AnimeInfo animeInfo)
        {
            var responseModel = new AnimeInfoCreationResponseModel
            {
                Id = animeInfo.Id,
                Title = animeInfo.Title?.Trim(),
                Description = animeInfo.Description?.Trim(),
                IsNsfw = animeInfo.IsNsfw
            };
            return responseModel;
        }
    }
}
