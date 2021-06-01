using AnimeBrowser.Common.Attributes;
using AnimeBrowser.Common.Models.BaseModels.MainModels;
using System;

namespace AnimeBrowser.Common.Models.ResponseModels.MainModels
{
    [ToJsonString]
    public partial class SeasonEditingResponseModel : SeasonResponseModel
    {
        public SeasonEditingResponseModel(long id, int seasonNumber, string title, string description, DateTime? startDate, DateTime? endDate,
                                            int airStatus, int? numberOfEpisodes, byte[] coverCarousel, byte[] cover, long animeInfoId,
                                             bool isAnimeInfoActive, bool isActive = true) :
            base(id: id, seasonNumber: seasonNumber, title: title, description: description, startDate: startDate, endDate: endDate,
                                            airStatus: airStatus, numberOfEpisodes: numberOfEpisodes, coverCarousel: coverCarousel, cover: cover, animeInfoId: animeInfoId,
                                            isActive: isActive, isAnimeInfoActive: isAnimeInfoActive)
        {
        }
    }
}
