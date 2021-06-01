using AnimeBrowser.Common.Attributes;

namespace AnimeBrowser.Common.Models.BaseModels.SecondaryModels
{
    [ToJsonString]
    public partial class SeasonGenreResponseModel
    {
        public SeasonGenreResponseModel(long id, long genreId, long seasonId)
        {
            this.Id = id;
            this.GenreId = genreId;
            this.SeasonId = seasonId;
        }

        public long Id { get; set; }
        public long GenreId { get; set; }
        public long SeasonId { get; set; }
    }
}
