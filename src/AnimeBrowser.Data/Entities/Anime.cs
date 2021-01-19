using System;
using System.Collections.Generic;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class Anime
    {
        public Anime()
        {
            AnimeEpisodes = new HashSet<AnimeEpisode>();
            AnimeGenres = new HashSet<AnimeGenre>();
            AnimeNames = new HashSet<AnimeName>();
            AnimeRatings = new HashSet<AnimeRating>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? AirStatus { get; set; }
        public string Description { get; set; }
        public bool? IsReboot { get; set; }
        public bool? IsRemake { get; set; }
        public bool? IsRemaster { get; set; }

        public virtual ICollection<AnimeEpisode> AnimeEpisodes { get; set; }
        public virtual ICollection<AnimeGenre> AnimeGenres { get; set; }
        public virtual ICollection<AnimeName> AnimeNames { get; set; }
        public virtual ICollection<AnimeRating> AnimeRatings { get; set; }
    }
}
