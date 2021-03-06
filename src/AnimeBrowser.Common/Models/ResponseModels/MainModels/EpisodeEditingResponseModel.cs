﻿using AnimeBrowser.Common.Attributes;
using AnimeBrowser.Common.Models.BaseModels.MainModels;
using AnimeBrowser.Common.Models.Enums;
using System;

namespace AnimeBrowser.Common.Models.ResponseModels.MainModels
{
    [ToJsonString]
    public partial class EpisodeEditingResponseModel : EpisodeResponseModel
    {
        public EpisodeEditingResponseModel(long id, int episodeNumber, AirStatuses airStatus, byte[] cover, DateTime? airDate, long animeInfoId,
            long seasonId, bool isAnimeInfoActive, bool isSeasonActive, string title = "", string description = "", bool isActive = true)
            : base(id: id, episodeNumber: episodeNumber, airStatus: airStatus, cover: cover, airDate: airDate, animeInfoId: animeInfoId,
                  seasonId: seasonId, title: title, description: description,
                  isActive: isActive, isSeasonActive: isSeasonActive, isAnimeInfoActive: isAnimeInfoActive)
        {
        }
    }
}
