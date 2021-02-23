using AnimeBrowser.Common.Models.BaseModels;
using AnimeBrowser.Common.Models.Enums;
using System;

namespace AnimeBrowser.Common.Models.ResponseModels
{
    public class EpisodeCreationResponseModel : EpisodeResponseModel
    {
        public EpisodeCreationResponseModel(long id, int episodeNumber, AirStatusEnum airStatus, byte[] cover, DateTime? airDate, long animeInfoId, long seasonId, string title = "", string description = "")
            : base(id: id, episodeNumber: episodeNumber, airStatus: airStatus, cover: cover, airDate: airDate, animeInfoId: animeInfoId, seasonId: seasonId, title: title, description: description)
        {
        }
    }
}
