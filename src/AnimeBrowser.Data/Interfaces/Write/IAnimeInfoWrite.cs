using AnimeBrowser.Common.Models.RequestModels;
using AnimeBrowser.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Write
{
    public interface IAnimeInfoWrite
    {
        Task<AnimeInfo> CreateAnimeInfo(AnimeInfo animeInfo);
        Task<AnimeInfo> UpdateAnimeInfo(AnimeInfo animeInfo);
    }
}
