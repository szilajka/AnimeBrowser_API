﻿using AnimeBrowser.Common.Attributes;
using System;
using System.Collections.Generic;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    [ToJsonString]
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
        public int SeasonNumber { get; set; }
        public decimal? Rating { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int AirStatus { get; set; }
        public int? NumberOfEpisodes { get; set; }
        public byte[] CoverCarousel { get; set; }
        public byte[] Cover { get; set; }
        public string Title { get; set; }
        public long AnimeInfoId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAnimeInfoActive { get; set; }

        public virtual AnimeInfo AnimeInfo { get; set; }
        public virtual ICollection<Episode> Episodes { get; set; }
        public virtual ICollection<SeasonGenre> SeasonGenres { get; set; }
        public virtual ICollection<SeasonMediaList> SeasonMediaLists { get; set; }
        public virtual ICollection<SeasonName> SeasonNames { get; set; }
        public virtual ICollection<SeasonRating> SeasonRatings { get; set; }
    }
}
