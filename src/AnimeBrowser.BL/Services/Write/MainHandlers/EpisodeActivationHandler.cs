﻿using AnimeBrowser.BL.Interfaces.Write.MainInterfaces;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.MainInterfaces;
using AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces;
using AnimeBrowser.Data.Interfaces.Write.MainInterfaces;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write.MainHandlers
{
    public class EpisodeActivationHandler : IEpisodeActivation
    {
        private readonly ILogger logger = Log.ForContext<EpisodeActivationHandler>();
        private readonly IEpisodeRead episodeReadRepo;
        private readonly IEpisodeRatingRead episodeRatingReadRepo;
        private readonly IEpisodeWrite episodeWriteRepo;

        public EpisodeActivationHandler(IEpisodeRead episodeReadRepo, IEpisodeRatingRead episodeRatingReadRepo, IEpisodeWrite episodeWriteRepo)
        {
            this.episodeReadRepo = episodeReadRepo;
            this.episodeRatingReadRepo = episodeRatingReadRepo;
            this.episodeWriteRepo = episodeWriteRepo;
        }

        public async Task<Episode> Activate(long episodeId)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. [{nameof(episodeId)}]: [{episodeId}].");

                if (episodeId <= 0)
                {
                    var error = new ErrorModel(code: ErrorCodes.OutOfRangeProperty.GetIntValueAsString(), description: $"The given id [{episodeId}] is not a valid id. A valid id must be greater than 0!",
                        source: nameof(episodeId), title: ErrorCodes.OutOfRangeProperty.GetDescription());
                    throw new NotExistingIdException(error, $"The given {nameof(Episode)}'s id is less than/equal to 0!");
                }

                var episode = await episodeReadRepo.GetEpisodeById(episodeId);
                if (episode == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                        description: $"No {nameof(Episode)} object was found with the given id [{episodeId}]!",
                        source: nameof(episodeId), title: ErrorCodes.EmptyObject.GetDescription()
                    );
                    throw new NotFoundObjectException<Episode>(error, $"Not found an {nameof(Episode)} entity with id: [{episodeId}].");
                }

                var episodeRatings = episodeRatingReadRepo.GetEpisodeRatingsByEpisodeId(episodeId);
                if (episodeRatings?.Any() == true)
                {
                    foreach (var er in episodeRatings)
                    {
                        er.IsEpisodeActive = true;
                    }
                }
                episode.IsActive = true;
                await episodeWriteRepo.UpdateEpisodeAndRatings(episode, episodeRatings);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");
                return episode;
            }
            catch (NotExistingIdException ex)
            {
                logger.Warning(ex, $"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
            catch (NotFoundObjectException<Episode> ex)
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
