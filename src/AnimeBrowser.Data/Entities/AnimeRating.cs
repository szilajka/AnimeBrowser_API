#nullable disable

using AnimeBrowser.Data.Entities.Identity;

namespace AnimeBrowser.Data.Entities
{
    public partial class AnimeRating
    {
        public long Id { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
        public long AnimeId { get; set; }
        public string UserId { get; set; }

        public virtual Anime Anime { get; set; }
        public virtual User User { get; set; }
    }
}
