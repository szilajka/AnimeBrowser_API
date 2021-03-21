using AnimeBrowser.Common.Models.Enums;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.BaseModels.MainModels
{
    public class EpisodeResponseModel
    {
        public EpisodeResponseModel(long id, int episodeNumber, AirStatuses airStatus, byte[] cover, DateTime? airDate, long animeInfoId, long seasonId, string title = "", string description = "")
        {
            this.Id = id;
            this.EpisodeNumber = episodeNumber;
            this.AirStatus = airStatus;
            this.Title = title;
            this.Description = description;
            this.Cover = cover;
            this.AirDate = airDate;
            this.AnimeInfoId = animeInfoId;
            this.SeasonId = seasonId;
        }

        public long Id { get; set; }
        public int EpisodeNumber { get; set; }
        public AirStatuses AirStatus { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] Cover { get; set; }
        public DateTime? AirDate { get; set; }
        public long AnimeInfoId { get; set; }
        public long SeasonId { get; set; }

        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        });
    }
}
