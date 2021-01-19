using System;
using System.Collections.Generic;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class AnimeEpisode
    {
        public AnimeEpisode()
        {
            AnimeEpisodeRatings = new HashSet<AnimeEpisodeRating>();
            EpisodeUserLists = new HashSet<EpisodeUserList>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public int EpisodeNumber { get; set; }
        public int? AirStatus { get; set; }
        public DateTime? AirDate { get; set; }
        public decimal? Rating { get; set; }
        public string Description { get; set; }
        public byte[] Cover { get; set; }
        public long AnimeId { get; set; }
        public long SeasonId { get; set; }

        public virtual Anime Anime { get; set; }
        public virtual ICollection<AnimeEpisodeRating> AnimeEpisodeRatings { get; set; }
        public virtual ICollection<EpisodeUserList> EpisodeUserLists { get; set; }
    }
}
