using AnimeBrowser.Common.Attributes;
using AnimeBrowser.Common.Models.Enums;
using System;

namespace AnimeBrowser.Common.Models.BaseModels.MainModels
{
    [ToJsonString]
    public partial class EpisodeResponseModel
    {
        public EpisodeResponseModel(long id, int episodeNumber, AirStatuses airStatus, byte[] cover, DateTime? airDate, long animeInfoId, long seasonId, bool isSeasonActive, bool isAnimeInfoActive,
            string title = "", string description = "", bool isActive = true)
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
            this.IsActive = isActive;
            this.IsSeasonActive = isSeasonActive;
            this.IsAnimeInfoActive = isAnimeInfoActive;
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
        public bool IsActive { get; set; }
        public bool IsSeasonActive { get; set; }
        public bool IsAnimeInfoActive { get; set; }
    }
}
