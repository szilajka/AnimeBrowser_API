using AnimeBrowser.Common.Models.BaseModels.MainModels;
using AnimeBrowser.Common.Models.Enums;
using System;

namespace AnimeBrowser.Common.Models.RequestModels.MainModels
{
    public partial class SeasonEditingRequestModel : SeasonRequestModel
    {
        public SeasonEditingRequestModel(long id, int seasonNumber, string title, string description, DateTime? startDate, DateTime? endDate,
                 AirStatuses airStatus, int? numberOfEpisodes, byte[] coverCarousel, byte[] cover, long animeInfoId)
         : base(seasonNumber: seasonNumber, title: title, description: description, startDate: startDate, endDate: endDate,
                 airStatus: airStatus, numberOfEpisodes: numberOfEpisodes, coverCarousel: coverCarousel, cover: cover, animeInfoId: animeInfoId)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
