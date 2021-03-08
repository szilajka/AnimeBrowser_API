using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.MainInterfaces
{
    public interface IEpisodeDelete
    {
        Task DeleteEpisode(long episodeId);
    }
}
