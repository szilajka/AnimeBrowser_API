using AnimeBrowser.Common.Models.BaseModels;
using AnimeBrowser.Common.Models.Enums;
using System;

namespace AnimeBrowser.Common.Models.RequestModels
{
    public class SeasonCreationRequestModel : SeasonRequestModel
    {
        public SeasonCreationRequestModel(int seasonNumber, string title, string description, DateTime? startDate, DateTime? endDate,
                    AirStatusEnum airStatus, int? numberOfEpisodes, byte[] coverCarousel, byte[] cover, long animeInfoId)
            : base(seasonNumber: seasonNumber, title: title, description: description, startDate: startDate, endDate: endDate,
                    airStatus: airStatus, numberOfEpisodes: numberOfEpisodes, coverCarousel: coverCarousel, cover: cover, animeInfoId: animeInfoId)
        {
        }
    }
}
