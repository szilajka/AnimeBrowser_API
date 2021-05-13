using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.MainInterfaces
{
    public interface ISeasonActivation
    {
        Task<Season> Activate(long seasonId);
    }
}
