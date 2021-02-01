using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;
using System;
using System.Collections.Generic;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class Episode : Identifiable<long>
    {
        public Episode()
        {
            EpisodeMediaLists = new HashSet<EpisodeMediaList>();
            EpisodeRatings = new HashSet<EpisodeRating>();
        }

        //public long Id { get; set; }
        [Attr]
        public int EpisodeNumber { get; set; }
        [Attr]
        public int AirStatus { get; set; }
        public long AnimeInfoId { get; set; }
        [Attr]
        public string Title { get; set; }
        [Attr]
        public decimal? Rating { get; set; }
        [Attr]
        public string Description { get; set; }
        [Attr]
        public byte[] Cover { get; set; }
        [Attr]
        public DateTime? AirDate { get; set; }
        public long SeasonId { get; set; }

        [HasOne]
        public virtual AnimeInfo AnimeInfo { get; set; }
        [HasOne]
        public virtual Season Season { get; set; }
        [HasMany]
        public virtual ICollection<EpisodeMediaList> EpisodeMediaLists { get; set; }
        [HasMany]
        public virtual ICollection<EpisodeRating> EpisodeRatings { get; set; }
    }
}
