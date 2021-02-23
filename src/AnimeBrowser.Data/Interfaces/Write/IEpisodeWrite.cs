using AnimeBrowser.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Write
{
    public interface IEpisodeWrite
    {
        Task<Episode> CreateEpisode(Episode episode);
    }
}
