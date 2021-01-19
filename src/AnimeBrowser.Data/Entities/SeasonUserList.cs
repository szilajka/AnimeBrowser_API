#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class SeasonUserList
    {
        public long Id { get; set; }
        public long SeasonId { get; set; }
        public long ListId { get; set; }

        public virtual AnimeList List { get; set; }
        public virtual Season Season { get; set; }
    }
}
