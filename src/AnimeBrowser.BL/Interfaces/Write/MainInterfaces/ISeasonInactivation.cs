using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.MainInterfaces
{
    public interface ISeasonInactivation
    {
        Task<Season> Inactivate(long seasonId);
    }
}
