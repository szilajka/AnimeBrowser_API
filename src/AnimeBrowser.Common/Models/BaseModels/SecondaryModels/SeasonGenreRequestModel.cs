using AnimeBrowser.Common.Attributes;

namespace AnimeBrowser.Common.Models.BaseModels.SecondaryModels
{
    [ToJsonString]
    public partial class SeasonGenreRequestModel
    {
        public SeasonGenreRequestModel(long genreId, long seasonId)
        {
            this.GenreId = genreId;
            this.SeasonId = seasonId;
        }

        public long GenreId { get; set; }
        public long SeasonId { get; set; }
    }
}
