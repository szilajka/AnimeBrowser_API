using AnimeBrowser.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Read.MainInterfaces
{
    public interface ISeasonRead
    {
        Task<Season?> GetSeasonById(long id);
        IList<Season> GetSeasonsByIds(IEnumerable<long> seasonIds);
        bool IsExistsSeasonWithSeasonNumber(long animeInfoId, int seasonNumber);
        bool IsExistsSeasonWithSeasonNumber(long seasonId, long animeInfoId, int seasonNumber);
        IEnumerable<Season>? GetSeasonsByAnimeInfoId(long animeInfoId);
    }
}
