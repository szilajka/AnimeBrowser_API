using AnimeBrowser.Data.Entities;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.MainInterfaces
{
    public interface IEpisodeActivation
    {
        Task<Episode> Activate(long episodeId);
    }
}
