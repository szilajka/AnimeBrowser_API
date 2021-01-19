using System;
using System.Collections.Generic;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class Season
    {
        public Season()
        {
            SeasonNames = new HashSet<SeasonName>();
            SeasonRatings = new HashSet<SeasonRating>();
            SeasonUserLists = new HashSet<SeasonUserList>();
        }

        public long Id { get; set; }
        public int SeasonNumber { get; set; }
        public string Description { get; set; }
        public byte[] Cover { get; set; }
        public byte[] CoverCarousel { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? AirStatus { get; set; }
        public int? NumberOfEpisodes { get; set; }
        public decimal? Rating { get; set; }
        public long AnimeId { get; set; }

        public virtual ICollection<SeasonName> SeasonNames { get; set; }
        public virtual ICollection<SeasonRating> SeasonRatings { get; set; }
        public virtual ICollection<SeasonUserList> SeasonUserLists { get; set; }
    }
}
