using AnimeBrowser.Common.Models.BaseModels.SecondaryModels;

namespace AnimeBrowser.Common.Models.RequestModels.SecondaryModels
{
    public partial class SeasonGenreCreationRequestModel : SeasonGenreRequestModel
    {
        public SeasonGenreCreationRequestModel(long genreId, long seasonId)
            : base(genreId: genreId, seasonId: seasonId)
        {
        }
    }
}
