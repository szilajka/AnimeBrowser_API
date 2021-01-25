using System;
using System.Collections.Generic;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class Episode
    {
        public Episode()
        {
            EpisodeMediaLists = new HashSet<EpisodeMediaList>();
            EpisodeRatings = new HashSet<EpisodeRating>();
        }

        public long Id { get; set; }
        public int EpisodeNumber { get; set; }
        public int AirStatus { get; set; }
        public long AnimeInfoId { get; set; }
        public string Title { get; set; }
        public decimal? Rating { get; set; }
        public string Description { get; set; }
        public byte[] Cover { get; set; }
        public DateTime? AirDate { get; set; }
        public long SeasonId { get; set; }

        public virtual AnimeInfo AnimeInfo { get; set; }
        public virtual Season Season { get; set; }
        public virtual ICollection<EpisodeMediaList> EpisodeMediaLists { get; set; }
        public virtual ICollection<EpisodeRating> EpisodeRatings { get; set; }
    }
}
