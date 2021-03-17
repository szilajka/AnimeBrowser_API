using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Write.SecondaryInterfaces
{
    public interface ISeasonRatingWrite
    {
        Task<SeasonRating> CreateSeasonRating(SeasonRating seasonRating);
        Task<SeasonRating> UpdateSeasonRating(SeasonRating seasonRating);
        Task DeleteSeasonRating(SeasonRating seasonRating);
    }
}
