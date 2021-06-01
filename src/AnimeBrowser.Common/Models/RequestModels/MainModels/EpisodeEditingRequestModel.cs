using AnimeBrowser.Common.Models.BaseModels.MainModels;
using AnimeBrowser.Common.Models.Enums;
using System;

namespace AnimeBrowser.Common.Models.RequestModels.MainModels
{
    public partial class EpisodeEditingRequestModel : EpisodeRequestModel
    {
        public EpisodeEditingRequestModel(long id, int episodeNumber, AirStatuses airStatus, byte[] cover, DateTime? airDate, long animeInfoId, long seasonId, string title = "", string description = "")
            : base(episodeNumber: episodeNumber, airStatus: airStatus, cover: cover, airDate: airDate, animeInfoId: animeInfoId, seasonId: seasonId, title: title, description: description)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
