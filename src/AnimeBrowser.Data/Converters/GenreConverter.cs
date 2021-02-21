using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Common.Models.ResponseModels;
using AnimeBrowser.Data.Entities;

namespace AnimeBrowser.Data.Converters
{
    public static class GenreConverter
    {
        #region RequestModel

        public static Genre ToGenre(this GenreCreationRequestModel requestModel)
        {
            var genre = new Genre
            {
                GenreName = requestModel.GenreName,
                Description = requestModel.Description
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

        #endregion ResponseModel
    }
}
