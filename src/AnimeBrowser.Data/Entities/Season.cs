using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class Season : Identifiable<long>
    {
        public Season()
        {
            Episodes = new HashSet<Episode>();
            SeasonGenres = new HashSet<SeasonGenre>();
            SeasonMediaLists = new HashSet<SeasonMediaList>();
            SeasonNames = new HashSet<SeasonName>();
            SeasonRatings = new HashSet<SeasonRating>();
        }

        //public long Id { get; set; }
        [Attr]
        [Required]
        [Range(1, 5000)]
        public int SeasonNumber { get; set; }

        [Attr]
        public decimal? Rating { get; set; }

        [Attr]
        [MaxLength(30000)]
        public string Description { get; set; }

        [Attr]
        public DateTime? StartDate { get; set; }

        [Attr]
        public DateTime? EndDate { get; set; }

        [Attr]
        [Required]
        public int AirStatus { get; set; }

        [Attr]
        public int? NumberOfEpisodes { get; set; }

        [Attr]
        public byte[] CoverCarousel { get; set; }

        [Attr]
        public byte[] Cover { get; set; }

        [Attr]
        [Required]
        [MinLength(2)]
        [MaxLength(255)]
        public string Title { get; set; }

        [Attr]
        [Required]
        public long AnimeInfoId { get; set; }

        [HasOne]
        public virtual AnimeInfo AnimeInfo { get; set; }
        [HasMany]
        public virtual ICollection<Episode> Episodes { get; set; }
        [HasMany]
        public virtual ICollection<SeasonGenre> SeasonGenres { get; set; }
        [HasMany]
        public virtual ICollection<SeasonMediaList> SeasonMediaLists { get; set; }
        [HasMany]
        public virtual ICollection<SeasonName> SeasonNames { get; set; }
        [HasMany]
        public virtual ICollection<SeasonRating> SeasonRatings { get; set; }
    }
}
