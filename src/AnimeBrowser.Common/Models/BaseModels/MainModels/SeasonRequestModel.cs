using AnimeBrowser.Common.Attributes;
using AnimeBrowser.Common.Models.Enums;
using System;

namespace AnimeBrowser.Common.Models.BaseModels.MainModels
{
    [ToJsonString]
    public partial class SeasonRequestModel
    {
        public SeasonRequestModel(int seasonNumber, string title, string description, DateTime? startDate, DateTime? endDate,
                                    AirStatuses airStatus, int? numberOfEpisodes, byte[] coverCarousel, byte[] cover, long animeInfoId)
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

        public AirStatuses AirStatus { get; set; }

        public int? NumberOfEpisodes { get; set; }

        public byte[] CoverCarousel { get; set; }

        public byte[] Cover { get; set; }

        public long AnimeInfoId { get; set; }
    }
}
