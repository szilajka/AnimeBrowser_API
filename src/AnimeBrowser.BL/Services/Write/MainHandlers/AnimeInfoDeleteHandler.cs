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
    public class AnimeInfoDeleteHandler : IAnimeInfoDelete
    {
        private readonly ILogger logger = Log.ForContext<AnimeInfoDeleteHandler>();
        private readonly IAnimeInfoRead animeInfoReadRepo;
        private readonly ISeasonRead seasonReadRepo;
        private readonly IEpisodeRead episodeReadRepo;
        private readonly ISeasonRatingRead seasonRatingReadRepo;
        private readonly IEpisodeRatingRead episodeRatingReadRepo;
        private readonly IAnimeInfoWrite animeInfoWriteRepo;

        public AnimeInfoDeleteHandler(IAnimeInfoRead animeInfoReadRepo, ISeasonRead seasonReadRepo, IEpisodeRead episodeReadRepo,
            ISeasonRatingRead seasonRatingReadRepo, IEpisodeRatingRead episodeRatingReadRepo, IAnimeInfoWrite animeInfoWriteRepo)
        {
            this.animeInfoReadRepo = animeInfoReadRepo;
            this.seasonReadRepo = seasonReadRepo;
            this.episodeReadRepo = episodeReadRepo;
            this.seasonRatingReadRepo = seasonRatingReadRepo;
            this.episodeRatingReadRepo = episodeRatingReadRepo;
            this.animeInfoWriteRepo = animeInfoWriteRepo;
        }

        public async Task DeleteAnimeInfo(long id)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(id)}: [{id}].");

                if (id <= 0)
                {
                    var error = new ErrorModel(code: ErrorCodes.OutOfRangeProperty.GetIntValueAsString(), description: $"The given id [{id}] is not a valid id. A valid id must be greater than 0!",
                        source: nameof(id), title: ErrorCodes.OutOfRangeProperty.GetDescription());
                    throw new NotExistingIdException(error, $"The given {nameof(AnimeInfo)}'s id is less than/equal to 0!");
                }

                var animeInfo = await animeInfoReadRepo.GetAnimeInfoById(id);
                if (animeInfo == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                         description: $"No {nameof(AnimeInfo)} object was found with the given id [{id}]!",
                         source: nameof(id), title: ErrorCodes.EmptyObject.GetDescription()
                     );
                    throw new NotFoundObjectException<AnimeInfo>(error, $"Not found an {nameof(AnimeInfo)} entity with id: [{id}].");
                }

                var seasons = seasonReadRepo.GetSeasonsByAnimeInfoId(id);
                var seasonIds = seasons?.Select(s => s.Id).Distinct();
                IEnumerable<Episode>? episodes = null;
                IEnumerable<SeasonRating>? seasonRatings = null;
                IEnumerable<EpisodeRating>? episodeRatings = null;
                if (seasonIds?.Any() == true)
                {
                    episodes = episodeReadRepo.GetEpisodesBySeasonIds(seasonIds);
                    var episodeIds = episodes?.Select(e => e.Id)?.Distinct();
                    if (episodeIds?.Any() == true)
                    {
                        episodeRatings = episodeRatingReadRepo.GetEpisodeRatingsByEpisodeIds(episodeIds);
                    }
                    seasonRatings = seasonRatingReadRepo.GetSeasonRatingsBySeasonIds(seasonIds);
                }

                await animeInfoWriteRepo.DeleteAnimeInfo(animeInfo, seasons, episodes, seasonRatings, episodeRatings);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");
            }
            catch (NotExistingIdException noIdEx)
            {
                logger.Warning(noIdEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{noIdEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<AnimeInfo> nfoEx)
            {
                logger.Warning(nfoEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{nfoEx.Message}].");
                throw;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{ex.Message}].");
                throw;
            }
        }
    }
}
