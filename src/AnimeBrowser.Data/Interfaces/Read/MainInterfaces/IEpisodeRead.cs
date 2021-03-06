﻿using AnimeBrowser.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Read.MainInterfaces
{
    public interface IEpisodeRead
    {
        Task<Episode?> GetEpisodeById(long episodeId);
        bool IsEpisodeWithEpisodeNumberExists(long seasonId, int episodeNumber);
        Task<bool> IsSeasonAndAnimeInfoExistsAndReferences(long seasonId, long animeInfoId);
        IEnumerable<Episode>? GetEpisodesBySeasonId(long seasonId);
        IEnumerable<Episode>? GetEpisodesBySeasonIds(IEnumerable<long>? seasonIds);
    }
}
