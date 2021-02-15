using AnimeBrowser.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Read
{
    public interface IAnimeInfoRead
    {
        Task<AnimeInfo> GetAnimeInfoById(long id);
    }
}
