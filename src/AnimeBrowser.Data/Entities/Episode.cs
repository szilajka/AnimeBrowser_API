using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        [Range(1, 5000)]
        public int EpisodeNumber { get; set; }

        [Required]
        public int AirStatus { get; set; }

        [Required]
        public long AnimeInfoId { get; set; }

        [MaxLength(255)]
        public string Title { get; set; }

        public decimal? Rating { get; set; }

        [MaxLength(30000)]
        public string Description { get; set; }

        public byte[] Cover { get; set; }

        public DateTime? AirDate { get; set; }

        [Required]
        public long SeasonId { get; set; }

        public virtual AnimeInfo AnimeInfo { get; set; }
        public virtual Season Season { get; set; }
        public virtual ICollection<EpisodeMediaList> EpisodeMediaLists { get; set; }
        public virtual ICollection<EpisodeRating> EpisodeRatings { get; set; }
    }
}
