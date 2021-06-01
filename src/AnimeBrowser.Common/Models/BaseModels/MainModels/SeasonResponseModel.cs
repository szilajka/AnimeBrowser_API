using AnimeBrowser.Common.Attributes;
using System;

namespace AnimeBrowser.Common.Models.BaseModels.MainModels
{
    [ToJsonString]
    public partial class SeasonResponseModel
    {
        public SeasonResponseModel(long id, int seasonNumber, string title, string description, DateTime? startDate, DateTime? endDate,
                                    int airStatus, int? numberOfEpisodes, byte[] coverCarousel, byte[] cover, long animeInfoId,
                                    bool isAnimeInfoActive, bool isActive = true)
        {
            this.Id = id;
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
            this.IsActive = isActive;
            this.IsAnimeInfoActive = isAnimeInfoActive;
        }

        public long Id { get; set; }

        public int SeasonNumber { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int AirStatus { get; set; }

        public int? NumberOfEpisodes { get; set; }

        public byte[] CoverCarousel { get; set; }

        public byte[] Cover { get; set; }

        public long AnimeInfoId { get; set; }

        public bool IsActive { get; set; }

        public bool IsAnimeInfoActive { get; set; }
    }
}
