using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Read
{
    public interface ISeasonRead
    {
        Task<Season?> GetSeasonById(long id);
        bool IsExistsSeasonWithSeasonNumber(long animeInfoId, int seasonNumber);
    }
}
