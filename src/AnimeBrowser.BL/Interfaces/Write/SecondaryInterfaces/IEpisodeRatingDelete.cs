using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces
{
    public interface IEpisodeRatingDelete
    {
        Task DeleteEpisodeRating(long id, IEnumerable<Claim>? claims);
    }
}
