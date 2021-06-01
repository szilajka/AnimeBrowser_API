using AnimeBrowser.Common.Attributes;

namespace AnimeBrowser.Common.Models.BaseModels.SecondaryModels
{
    [ToJsonString]
    public partial class AnimeInfoNameRequestModel
    {
        public AnimeInfoNameRequestModel(long animeInfoId, string title = "")
        {
            this.Title = title;
            this.AnimeInfoId = animeInfoId;
        }

        public string Title { get; set; }
        public long AnimeInfoId { get; set; }
    }
}
