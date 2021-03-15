using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces
{
    public interface ISeasonGenreDelete
    {
        Task DeleteSeasonGenres(IList<long> seasonGenreIds);
    }
}
