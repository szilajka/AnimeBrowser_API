﻿using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.MainInterfaces;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Read.MainRepositories
{
    public class SeasonReadRepository : ISeasonRead
    {
        private readonly AnimeBrowserContext abContext;
        private readonly ILogger logger = Log.ForContext<SeasonReadRepository>();

        public SeasonReadRepository(AnimeBrowserContext context)
        {
            abContext = context;
        }

        public async Task<Season?> GetSeasonById(long seasonId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonId)}: [{seasonId}].");

            var season = await abContext.Seasons.FindAsync(seasonId);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(Season)}?.{nameof(Season.Id)}: [{season?.Id}].");
            return season;
        }

        public IList<Season> GetSeasonsByIds(IEnumerable<long> seasonIds)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonIds)}: [{string.Join(", ", seasonIds)}].");

            var foundSeasons = abContext.Seasons.ToList().Where(s => seasonIds.Contains(s.Id)).ToList();

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(Season)}.Count: [{foundSeasons.Count}].");
            return foundSeasons;
        }

        public bool IsExistsSeasonWithSeasonNumber(long animeInfoId, int seasonNumber)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(animeInfoId)}: [{animeInfoId}], {nameof(seasonNumber)}: [{nameof(seasonNumber)}].");

            var result = abContext.Seasons.ToList().Any(s => s.AnimeInfoId == animeInfoId && s.SeasonNumber == seasonNumber);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(result)}: [{result}].");
            return result;
        }

        public bool IsExistsSeasonWithSeasonNumber(long seasonId, long animeInfoId, int seasonNumber)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonId)}: [{seasonId}], {nameof(animeInfoId)}: [{animeInfoId}], {nameof(seasonNumber)}: [{nameof(seasonNumber)}].");

            var result = abContext.Seasons.ToList().Any(s => s.Id != seasonId && s.AnimeInfoId == animeInfoId && s.SeasonNumber == seasonNumber);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(result)}: [{result}].");
            return result;
        }

        public IEnumerable<Season>? GetSeasonsByAnimeInfoId(long animeInfoId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(animeInfoId)}: [{animeInfoId}].");

            var foundSeasons = abContext.Seasons.ToList().Where(s => s.AnimeInfoId == animeInfoId);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. {nameof(foundSeasons)}.Count: [{foundSeasons?.Count()}].");
            return foundSeasons;
        }
    }
}
