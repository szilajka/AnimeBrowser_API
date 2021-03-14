using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.ResponseModels.SecondaryModels
{
    public class SeasonGenreCreationResponseModel : SeasonGenreResponseModel
    {
        public SeasonGenreCreationResponseModel(long id, long genreId, long seasonId)
            : base(id: id, genreId: genreId, seasonId: seasonId)
        {
        }
    }
}
