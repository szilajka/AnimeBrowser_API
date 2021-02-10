using AnimeBrowser.Data.Entities.Identity;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class SeasonRating 
    {
        public long Id { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
        public long SeasonId { get; set; }
        public string UserId { get; set; }

        public virtual Season Season { get; set; }
        public virtual User User { get; set; }
    }
}
