using AnimeBrowser.BL.Interfaces.Write.MainInterfaces;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.MainInterfaces;
using AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces;
using AnimeBrowser.Data.Interfaces.Write.MainInterfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write.MainHandlers
{
    public class AnimeInfoInactivationHandler : IAnimeInfoInactivation
    {
        private readonly ILogger logger = Log.ForContext<AnimeInfoInactivationHandler>();
        private readonly IAnimeInfoRead animeInfoReadRepo;
        private readonly ISeasonRead seasonReadRepo;
        private readonly ISeasonRatingRead seasonRatingReadRepo;
        private readonly IEpisodeRead episodeReadRepo;
        private readonly IEpisodeRatingRead episodeRatingReadRepo;
        private readonly IAnimeInfoWrite animeInfoWriteRepo;

        public AnimeInfoInactivationHandler(IAnimeInfoRead animeInfoReadRepo, ISeasonRead seasonReadRepo, ISeasonRatingRead seasonRatingReadRepo,
            IEpisodeRead episodeReadRepo, IEpisodeRatingRead episodeRatingReadRepo, IAnimeInfoWrite animeInfoWriteRepo)
        {
            this.animeInfoReadRepo = animeInfoReadRepo;
            this.seasonReadRepo = seasonReadRepo;
            this.seasonRatingReadRepo = seasonRatingReadRepo;
            this.episodeReadRepo = episodeReadRepo;
            this.episodeRatingReadRepo = episodeRatingReadRepo;
            this.animeInfoWriteRepo = animeInfoWriteRepo;
        }

        public async Task<AnimeInfo> Inactivate(long animeInfoId)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. [{nameof(animeInfoId)}]: [{animeInfoId}].");
                if (animeInfoId <= 0)
                {
                    var error = new ErrorModel(code: ErrorCodes.OutOfRangeProperty.GetIntValueAsString(), description: $"The given id [{animeInfoId}] is not a valid id. A valid id must be greater than 0!",
                        source: nameof(animeInfoId), title: ErrorCodes.OutOfRangeProperty.GetDescription());
                    throw new NotExistingIdException(error, $"The given {nameof(AnimeInfo)}'s id is less than/equal to 0!");
                }

                var animeInfo = await animeInfoReadRepo.GetAnimeInfoById(animeInfoId);
                if (animeInfo == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                        description: $"No {nameof(AnimeInfo)} object was found with the given id [{animeInfoId}]!",
                        source: nameof(animeInfoId), title: ErrorCodes.EmptyObject.GetDescription()
                    );
                    throw new NotFoundObjectException<AnimeInfo>(error, $"Not found an {nameof(AnimeInfo)} entity with id: [{animeInfoId}].");
                }

                var seasons = seasonReadRepo.GetSeasonsByAnimeInfoId(animeInfoId);
                var seasonIds = seasons?.Select(s => s.Id)?.Distinct();
                var seasonRatings = seasonRatingReadRepo.GetSeasonRatingsBySeasonIds(seasonIds);
                var episodes = episodeReadRepo.GetEpisodesBySeasonIds(seasonIds);
                var episodeIds = episodes?.Select(e => e.Id)?.Distinct();
                var episodeRatings = episodeRatingReadRepo.GetEpisodeRatingsByEpisodeIds(episodeIds);
                if (episodeRatings?.Any() == true)
                {
                    foreach (var er in episodeRatings)
                    {
                        er.IsAnimeInfoActive = false;
                    }
                }
                if (episodes?.Any() == true)
                {
                    foreach (var e in episodes)
                    {
                        e.IsAnimeInfoActive = false;
                    }
                }
                if (seasonRatings?.Any() == true)
                {
                    foreach (var sr in seasonRatings)
                    {
                        sr.IsAnimeInfoActive = false;
                    }
                }
                if (seasons?.Any() == true)
                {
                    foreach (var s in seasons)
                    {
                        s.IsAnimeInfoActive = false;
                    }
                }
                animeInfo.IsActive = false;

                await animeInfoWriteRepo.UpdateAnimeInfoActiveStatus(animeInfo, seasons, seasonRatings, episodes, episodeRatings);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");
                return animeInfo;
            }
            catch (NotExistingIdException ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (NotFoundObjectException<AnimeInfo> ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
        }
    }
}
