﻿using AnimeBrowser.Common.Models.Enums;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AnimeBrowser.Common.Models.BaseModels
{
    public class EpisodeRequestModel
    {
        public EpisodeRequestModel(int episodeNumber, AirStatusEnum airStatus, byte[] cover, DateTime? airDate, long animeInfoId, long seasonId, string title = "", string description = "")
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
        public AirStatusEnum AirStatus { get; set; }
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
