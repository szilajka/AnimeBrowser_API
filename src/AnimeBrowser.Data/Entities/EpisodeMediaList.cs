#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class EpisodeMediaList
    {
        public long Id { get; set; }
        public long ListId { get; set; }
        public long EpisodeId { get; set; }

        public virtual Episode Episode { get; set; }
        public virtual MediaList List { get; set; }
    }
}
