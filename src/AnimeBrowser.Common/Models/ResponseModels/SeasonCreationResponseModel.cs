using AnimeBrowser.Common.Models.BaseModels;
using System;

namespace AnimeBrowser.Common.Models.ResponseModels
{
    public class SeasonCreationResponseModel : SeasonResponseModel
    {
        public SeasonCreationResponseModel(long id, int seasonNumber, string title, string description, DateTime? startDate, DateTime? endDate,
                                            int airStatus, int? numberOfEpisodes, byte[] coverCarousel, byte[] cover, long animeInfoId) :
            base(id: id, seasonNumber: seasonNumber, title: title, description: description, startDate: startDate, endDate: endDate,
                                            airStatus: airStatus, numberOfEpisodes: numberOfEpisodes, coverCarousel: coverCarousel, cover: cover, animeInfoId: animeInfoId)
        {
        }
    }
}
