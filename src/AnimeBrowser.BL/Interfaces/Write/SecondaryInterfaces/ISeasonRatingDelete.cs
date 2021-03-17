using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces
{
    public interface ISeasonRatingDelete
    {
        Task DeleteSeasonRating(long id);
    }
}
