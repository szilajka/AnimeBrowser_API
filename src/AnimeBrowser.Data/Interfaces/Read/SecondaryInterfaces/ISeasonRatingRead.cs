using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces
{
    public interface ISeasonRatingRead
    {
        Task<SeasonRating?> GetSeasonRatingById(long id);
        SeasonRating? GetSeasonRatingBySeasonAndUserId(long seasonId, string userId);
    }
}
