using AnimeBrowser.Common.Attributes;

namespace AnimeBrowser.Common.Models.BaseModels.SecondaryModels
{
    [ToJsonString]
    public partial class SeasonNameRequestModel
    {
        public SeasonNameRequestModel(string title, long seasonId)
        {
            this.Title = title;
            this.SeasonId = seasonId;
        }

        public string Title { get; set; }
        public long SeasonId { get; set; }
    }
}
