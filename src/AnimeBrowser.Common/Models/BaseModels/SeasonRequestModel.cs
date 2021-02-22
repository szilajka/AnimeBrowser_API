using AnimeBrowser.Common.Models.Enums;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.BaseModels
{
    public class SeasonRequestModel
    {
        public SeasonRequestModel(int seasonNumber, string title, string description, DateTime? startDate, DateTime? endDate,
                                    AirStatusEnum airStatus, int? numberOfEpisodes, byte[] coverCarousel, byte[] cover, long animeInfoId)
        {
            this.SeasonNumber = seasonNumber;
            this.Title = title;
            this.Description = description;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.AirStatus = airStatus;
            this.NumberOfEpisodes = numberOfEpisodes;
            this.CoverCarousel = coverCarousel;
            this.Cover = cover;
            this.AnimeInfoId = animeInfoId;
        }

        public int SeasonNumber { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public AirStatusEnum AirStatus { get; set; }

        public int? NumberOfEpisodes { get; set; }

        public byte[] CoverCarousel { get; set; }

        public byte[] Cover { get; set; }

        public long AnimeInfoId { get; set; }


        [ExcludeFromCodeCoverage]
        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        });
    }
}
