using AnimeBrowser.Common.Attributes;

namespace AnimeBrowser.Common.Models.BaseModels.MainModels
{
    [ToJsonString]
    public partial class AnimeInfoRequestModel
    {
        public AnimeInfoRequestModel(string title = "", string description = "", bool isNsfw = false)
        {
            this.Title = title;
            this.Description = description;
            this.IsNsfw = isNsfw;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsNsfw { get; set; }
    }
}
