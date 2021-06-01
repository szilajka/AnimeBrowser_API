using AnimeBrowser.Common.Models.BaseModels.MainModels;
using AnimeBrowser.Common.Models.Enums;
using System;


namespace AnimeBrowser.Common.Models.RequestModels.MainModels
{
    public partial class SeasonCreationRequestModel : SeasonRequestModel
    {
        public SeasonCreationRequestModel(int seasonNumber, string title, string description, DateTime? startDate, DateTime? endDate,
                    AirStatuses airStatus, int? numberOfEpisodes, byte[] coverCarousel, byte[] cover, long animeInfoId, bool isActive = true)
            : base(seasonNumber: seasonNumber, title: title, description: description, startDate: startDate, endDate: endDate,
                    airStatus: airStatus, numberOfEpisodes: numberOfEpisodes, coverCarousel: coverCarousel, cover: cover, animeInfoId: animeInfoId)
        {
            this.IsActive = isActive;
        }

        public bool IsActive { get; set; }
    }
}
