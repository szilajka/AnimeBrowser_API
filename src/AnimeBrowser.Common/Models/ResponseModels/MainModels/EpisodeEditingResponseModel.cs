using AnimeBrowser.Common.Models.BaseModels.MainModels;
using AnimeBrowser.Common.Models.Enums;
using System;

namespace AnimeBrowser.Common.Models.ResponseModels.MainModels
{
    public class EpisodeEditingResponseModel : EpisodeResponseModel
    {
        public EpisodeEditingResponseModel(long id, int episodeNumber, AirStatuses airStatus, byte[] cover, DateTime? airDate, long animeInfoId, long seasonId, string title = "", string description = "")
            : base(id: id, episodeNumber: episodeNumber, airStatus: airStatus, cover: cover, airDate: airDate, animeInfoId: animeInfoId, seasonId: seasonId, title: title, description: description)
        {
        }
    }
}
