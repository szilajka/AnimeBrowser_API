#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class AnimeInfoName
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long AnimeInfoId { get; set; }

        public virtual AnimeInfo AnimeInfo { get; set; }
    }
}
