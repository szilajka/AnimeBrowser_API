using AnimeBrowser.Common.Models.RequestModels.SecondaryModels;
using AnimeBrowser.Common.Models.ResponseModels.SecondaryModels;
using AnimeBrowser.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AnimeBrowser.Data.Converters.SecondaryConverters
{
    public static class SeasonGenreConverter
    {
        #region RequestModel
        public static SeasonGenre ToSeasonGenre(this SeasonGenreCreationRequestModel requestModel)
        {
            var seasonGenre = new SeasonGenre
            {
                SeasonId = requestModel.SeasonId,
                GenreId = requestModel.GenreId
            };
            return seasonGenre;
        }

        public static IList<SeasonGenre> ToSeasonGenres(this IList<SeasonGenreCreationRequestModel> requestModel)
        {
            List<SeasonGenre> seasonGenres = new();
            if (!requestModel.Any()) return seasonGenres;

            foreach (var rm in requestModel)
            {
                var seasonGenre = new SeasonGenre
                {
                    SeasonId = rm.SeasonId,
                    GenreId = rm.GenreId
                };
                seasonGenres.Add(seasonGenre);
            }
            return seasonGenres;
        }


        #endregion RequestModel

        #region ResponseModel
        public static IList<SeasonGenreCreationResponseModel> ToCreationResponseModel(this IEnumerable<SeasonGenre> seasonGenres)
        {
            List<SeasonGenreCreationResponseModel> responseModel = new();
            if (!seasonGenres.Any()) return responseModel;

            foreach (var sg in seasonGenres)
            {
                var rpm = new SeasonGenreCreationResponseModel(id: sg.Id, genreId: sg.GenreId, seasonId: sg.SeasonId);
                responseModel.Add(rpm);
            }
            return responseModel;
        }

        public static SeasonGenreCreationResponseModel ToCreationResponseModel(this SeasonGenre seasonGenre)
        {
            var responseModel = new SeasonGenreCreationResponseModel(id: seasonGenre.Id, seasonId: seasonGenre.SeasonId, genreId: seasonGenre.GenreId);
            return responseModel;
        }

        #endregion ResponseModel
    }
}
