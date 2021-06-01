using AnimeBrowser.Common.Attributes;
using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.ResponseModels.SecondaryModels
{
    [ToJsonString]
    public partial class SeasonGenreCreationResponseModel : SeasonGenreResponseModel
    {
        public SeasonGenreCreationResponseModel(long id, long genreId, long seasonId)
            : base(id: id, genreId: genreId, seasonId: seasonId)
        {
        }
    }
}
