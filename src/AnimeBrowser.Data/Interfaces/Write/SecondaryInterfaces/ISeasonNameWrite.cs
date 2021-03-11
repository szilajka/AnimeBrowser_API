using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Write.SecondaryInterfaces
{
    public interface ISeasonNameWrite
    {
        Task<SeasonName> CreateSeasonName(SeasonName seasonName);
        Task<SeasonName> UpdateSeasonName(SeasonName seasonName);
        Task DeleteSeasonName(SeasonName seasonName);
    }
}
