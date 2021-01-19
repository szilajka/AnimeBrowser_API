#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class AnimeGenre
    {
        public long Id { get; set; }
        public long GenreId { get; set; }
        public long AnimeId { get; set; }

        public virtual Anime Anime { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
