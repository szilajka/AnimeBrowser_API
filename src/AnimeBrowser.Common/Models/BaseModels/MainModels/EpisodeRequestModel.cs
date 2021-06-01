using AnimeBrowser.Common.Attributes;
using AnimeBrowser.Common.Models.Enums;
using System;

namespace AnimeBrowser.Common.Models.BaseModels.MainModels
{
    [ToJsonString]
    public partial class EpisodeRequestModel
    {
        public EpisodeRequestModel(int episodeNumber, AirStatuses airStatus, byte[] cover, DateTime? airDate, long animeInfoId, long seasonId, string title = "", string description = "")
        {
            this.EpisodeNumber = episodeNumber;
            this.AirStatus = airStatus;
            this.Title = title;
            this.Description = description;
            this.Cover = cover;
            this.AirDate = airDate;
            this.AnimeInfoId = animeInfoId;
            this.SeasonId = seasonId;
        }

        public int EpisodeNumber { get; set; }
        public AirStatuses AirStatus { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] Cover { get; set; }
        public DateTime? AirDate { get; set; }
        public long AnimeInfoId { get; set; }
        public long SeasonId { get; set; }
    }
}
