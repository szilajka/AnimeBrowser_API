using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

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
        public bool? IsActive { get; set; }
        public bool? IsAnimeInfoActive { get; set; }
        public bool? IsSeasonActive { get; set; }

        public virtual AnimeInfo AnimeInfo { get; set; }
        public virtual Season Season { get; set; }
        public virtual ICollection<EpisodeMediaList> EpisodeMediaLists { get; set; }
        public virtual ICollection<EpisodeRating> EpisodeRatings { get; set; }


        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
