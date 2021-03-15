using AnimeBrowser.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Write.SecondaryInterfaces
{
    public interface IEpisodeRatingWrite
    {
        Task<EpisodeRating> CreateEpisodeRating(EpisodeRating episodeRating);
    }
}
