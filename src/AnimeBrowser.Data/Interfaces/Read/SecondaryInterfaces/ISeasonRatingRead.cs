using AnimeBrowser.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces
{
    public interface ISeasonRatingRead
    {
        Task<SeasonRating?> GetSeasonRatingById(long id);
        SeasonRating? GetSeasonRatingBySeasonAndUserId(long seasonId, string userId);
        IEnumerable<SeasonRating>? GetSeasonRatingsBySeasonId(long seasonId);
        IEnumerable<SeasonRating>? GetSeasonRatingsBySeasonIds(IEnumerable<long>? seasonIds);
    }
}
