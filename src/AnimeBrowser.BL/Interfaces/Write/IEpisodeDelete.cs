using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write
{
    public interface IEpisodeDelete
    {
        Task DeleteEpisode(long episodeId);
    }
}
