namespace AnimeBrowser.Common.Models.RequestModels
{
    public class AnimeInfoCreationRequestModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsNsfw { get; set; }
    }
}
