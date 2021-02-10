using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class Season
    {
        public Season()
        {
            Episodes = new HashSet<Episode>();
            SeasonGenres = new HashSet<SeasonGenre>();
            SeasonMediaLists = new HashSet<SeasonMediaList>();
            SeasonNames = new HashSet<SeasonName>();
            SeasonRatings = new HashSet<SeasonRating>();
        }

        public long Id { get; set; }

        [Required]
        [Range(1, 5000)]
        public int SeasonNumber { get; set; }

        public decimal? Rating { get; set; }

        [MaxLength(30000)]
        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public int AirStatus { get; set; }

        public int? NumberOfEpisodes { get; set; }

        public byte[] CoverCarousel { get; set; }

        public byte[] Cover { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public long AnimeInfoId { get; set; }

        public virtual AnimeInfo AnimeInfo { get; set; }
        public virtual ICollection<Episode> Episodes { get; set; }
        public virtual ICollection<SeasonGenre> SeasonGenres { get; set; }
        public virtual ICollection<SeasonMediaList> SeasonMediaLists { get; set; }
        public virtual ICollection<SeasonName> SeasonNames { get; set; }
        public virtual ICollection<SeasonRating> SeasonRatings { get; set; }
    }
}
