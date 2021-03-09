using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces
{
    public interface IAnimeInfoNameDelete
    {
        Task DeleteAnimeInfoName(long id);
    }
}
