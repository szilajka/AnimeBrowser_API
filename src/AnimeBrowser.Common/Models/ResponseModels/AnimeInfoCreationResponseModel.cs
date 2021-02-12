namespace AnimeBrowser.Common.Models.ResponseModels
{
    public class AnimeInfoCreationResponseModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsNsfw { get; set; }
    }
}
