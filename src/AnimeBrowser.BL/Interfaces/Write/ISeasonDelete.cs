using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write
{
    public interface ISeasonDelete
    {
        Task DeleteSeason(long seasonId);
    }
}
