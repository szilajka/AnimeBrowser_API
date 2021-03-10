using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces
{
    public interface ISeasonNameRead
    {
        Task<SeasonName?> GetSeasonNameById(long id);
        bool IsExistsWithSameTitle(long id, string title, long seasonId);
        bool IsExistsWithSameTitle(string title, long seasonId);
    }
}
