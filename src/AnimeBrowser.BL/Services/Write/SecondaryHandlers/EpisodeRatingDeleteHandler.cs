using AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces;
using AnimeBrowser.Data.Interfaces.Write.SecondaryInterfaces;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write.SecondaryHandlers
{
    public class EpisodeRatingDeleteHandler : IEpisodeRatingDelete
    {
        private readonly ILogger logger = Log.ForContext<EpisodeRatingDeleteHandler>();
        private readonly IEpisodeRatingRead episodeRatingReadRepo;
        private readonly IEpisodeRatingWrite episodeRatingWriteRepo;

        public EpisodeRatingDeleteHandler(IEpisodeRatingRead episodeRatingReadRepo, IEpisodeRatingWrite episodeRatingWriteRepo)
        {
            this.episodeRatingReadRepo = episodeRatingReadRepo;
            this.episodeRatingWriteRepo = episodeRatingWriteRepo;
        }

        public async Task DeleteEpisodeRating(long id)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(id)}: [{id}].");

                if (id <= 0)
                {
                    var error = new ErrorModel(code: ErrorCodes.OutOfRangeProperty.GetIntValueAsString(), description: $"The given id [{id}] is not a valid id. A valid id must be greater than 0!",
                        source: nameof(id), title: ErrorCodes.OutOfRangeProperty.GetDescription());
                    throw new NotExistingIdException(error, $"The given {nameof(EpisodeRating)}'s id is less than/equal to 0!");
                }

                var episodeRating = await episodeRatingReadRepo.GetEpisodeRatingById(id);
                if (episodeRating == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                         description: $"No {nameof(EpisodeRating)} object was found with the given id [{id}]!",
                         source: nameof(id), title: ErrorCodes.EmptyObject.GetDescription()
                     );
                    throw new NotFoundObjectException<EpisodeRating>(error, $"Not found an {nameof(EpisodeRating)} entity with id: [{id}].");
                }

                await episodeRatingWriteRepo.DeleteEpisodeRating(episodeRating);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");
            }
            catch (NotExistingIdException noIdEx)
            {
                logger.Warning(noIdEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{noIdEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<EpisodeRating> nfoEx)
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
