using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.MainInterfaces
{
    public interface IEpisodeInactivation
    {
        Task<Episode> Inactivate(long episodeId);
    }
}
