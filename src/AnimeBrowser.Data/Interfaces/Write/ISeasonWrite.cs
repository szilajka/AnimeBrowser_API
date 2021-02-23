using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Write
{
    public interface ISeasonWrite
    {
        Task<Season> CreateSeason(Season season);
        Task<Season> UpdateSeason(Season season);
    }
}
