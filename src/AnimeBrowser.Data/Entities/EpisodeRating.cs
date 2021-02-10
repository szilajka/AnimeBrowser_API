using AnimeBrowser.Data.Entities.Identity;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class EpisodeRating
    {
        public long Id { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
        public long EpisodeId { get; set; }
        public string UserId { get; set; }

        public virtual Episode Episode { get; set; }
        public virtual User User { get; set; }
    }
}
