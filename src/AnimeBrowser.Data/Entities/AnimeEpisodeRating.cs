#nullable disable

using AnimeBrowser.Data.Entities.Identity;

namespace AnimeBrowser.Data.Entities
{
    public partial class AnimeEpisodeRating
    {
        public long Id { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
        public long EpisodeId { get; set; }
        public string UserId { get; set; }

        public virtual AnimeEpisode Episode { get; set; }
        public virtual User User { get; set; }
    }
}
