#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class AnimeName
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long AnimeId { get; set; }

        public virtual Anime Anime { get; set; }
    }
}
