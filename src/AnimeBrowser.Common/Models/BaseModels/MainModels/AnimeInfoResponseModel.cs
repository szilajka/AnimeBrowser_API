using AnimeBrowser.Common.Attributes;

namespace AnimeBrowser.Common.Models.BaseModels.MainModels
{
    [ToJsonString]
    public partial class AnimeInfoResponseModel
    {
        public AnimeInfoResponseModel(long id, string title = "", string description = "", bool isNsfw = false, bool isActive = true)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.IsNsfw = isNsfw;
            this.IsActive = isActive;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsNsfw { get; set; }
        public bool IsActive { get; set; }
    }
}
