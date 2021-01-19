#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class EpisodeUserList
    {
        public long Id { get; set; }
        public long EpisodeId { get; set; }
        public long ListId { get; set; }

        public virtual AnimeEpisode Episode { get; set; }
        public virtual AnimeList List { get; set; }
    }
}
