using AnimeBrowser.Common.Models.RequestModels.MainModels;
using AnimeBrowser.Common.Models.ResponseModels.MainModels;
using AnimeBrowser.Data.Entities;

namespace AnimeBrowser.Data.Converters.MainConverters
{
    public static class GenreConverter
    {
        #region RequestModel

        public static Genre ToGenre(this GenreCreationRequestModel requestModel)
        {
            var genre = new Genre
            {
                GenreName = requestModel.GenreName?.Trim(),
                Description = requestModel.Description?.Trim()
            };

            return genre;
        }

        public static Genre ToGenre(this GenreEditingRequestModel requestModel)
        {
            var genre = new Genre
            {
                Id = requestModel.Id,
                GenreName = requestModel.GenreName?.Trim(),
                Description = requestModel.Description?.Trim()
            };

            return genre;
        }

        #endregion RequestModel

        #region ResponseModel

        public static GenreCreationResponseModel ToCreationResponseModel(this Genre genre)
        {
            var responseModel = new GenreCreationResponseModel(id: genre.Id, genreName: genre.GenreName, description: genre.Description);
            return responseModel;
        }

        public static GenreEditingResponseModel ToEditingResponseModel(this Genre genre)
        {
            var responseModel = new GenreEditingResponseModel(id: genre.Id, genreName: genre.GenreName, description: genre.Description);
            return responseModel;
        }

        #endregion ResponseModel
    }
}
