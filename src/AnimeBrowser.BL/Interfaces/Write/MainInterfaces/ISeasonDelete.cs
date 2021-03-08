using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.MainInterfaces
{
    public interface ISeasonDelete
    {
        Task DeleteSeason(long seasonId);
    }
}
