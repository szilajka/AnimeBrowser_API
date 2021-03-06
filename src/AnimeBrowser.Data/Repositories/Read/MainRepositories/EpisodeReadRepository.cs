﻿using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.MainInterfaces;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Read.MainRepositories
{
    public class EpisodeReadRepository : IEpisodeRead
    {
        private readonly AnimeBrowserContext abContext;
        private readonly ILogger logger = Log.ForContext<EpisodeReadRepository>();

        public EpisodeReadRepository(AnimeBrowserContext context)
        {
            abContext = context;
        }

        public async Task<Episode?> GetEpisodeById(long episodeId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(episodeId)}: [{episodeId}].");

            var episode = await abContext.Episodes.FindAsync(episodeId);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(Episode)}?.{nameof(Episode.Id)}: [{episode?.Id}].");
            return episode;
        }

        public bool IsEpisodeWithEpisodeNumberExists(long seasonId, int episodeNumber)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonId)}: [{seasonId}], {nameof(episodeNumber)}: [{episodeNumber}].");
            var isExists = abContext.Episodes.ToList().Any(e => e.SeasonId == seasonId && e.EpisodeNumber == episodeNumber);
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. result: [{isExists}].");
            return isExists;
        }

        public async Task<bool> IsSeasonAndAnimeInfoExistsAndReferences(long seasonId, long animeInfoId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonId)}: [{seasonId}], {nameof(animeInfoId)}: [{animeInfoId}].");
            var result = false;
            var season = await abContext.Seasons.FindAsync(seasonId);
            if (season == null || season.AnimeInfoId != animeInfoId) return result;
            var animeInfo = await abContext.AnimeInfos.FindAsync(animeInfoId);
            if (animeInfo == null) return result;

            result = true;
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(result)}: [{result}].");
            return result;
        }

        public IEnumerable<Episode>? GetEpisodesBySeasonId(long seasonId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonId)}: [{seasonId}].");

            var episodes = abContext.Episodes.ToList().Where(e => e.SeasonId == seasonId);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(episodes)}.Count: [{episodes?.Count()}].");
            return episodes;
        }

        public IEnumerable<Episode>? GetEpisodesBySeasonIds(IEnumerable<long>? seasonIds)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started.");
            if (seasonIds?.Any() == true)
            {
                logger.Debug($"{nameof(seasonIds)}: [{string.Join(", ", seasonIds)}].");
                var episodes = abContext.Episodes.ToList().Where(e => seasonIds.Contains(e.SeasonId));
                logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(episodes)}.Count: [{episodes?.Count()}].");
                return episodes;
            }

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(seasonIds)} list is empty.");
            return Enumerable.Empty<Episode>();
        }
    }
}
