using AnimeBrowser.Common.Attributes;

namespace AnimeBrowser.Common.Models.BaseModels.SecondaryModels
{
    [ToJsonString]
    public partial class SeasonNameResponseModel
    {
        public SeasonNameResponseModel(long id, string title, long seasonId)
        {
            this.Id = id;
            this.Title = title;
            this.SeasonId = seasonId;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public long SeasonId { get; set; }
    }
}
