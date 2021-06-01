using AnimeBrowser.Common.Attributes;

namespace AnimeBrowser.Common.Models.BaseModels.SecondaryModels
{
    [ToJsonString]
    public partial class AnimeInfoNameResponseModel
    {
        public AnimeInfoNameResponseModel(long id, long animeInfoId, string title = "")
        {
            this.Id = id;
            this.Title = title;
            this.AnimeInfoId = animeInfoId;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public long AnimeInfoId { get; set; }
    }
}
