using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces
{
    public interface ISeasonNameDelete
    {
        Task DeleteSeasonName(long id);
    }
}
