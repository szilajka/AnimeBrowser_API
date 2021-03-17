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
    public class SeasonRatingDeleteHandler : ISeasonRatingDelete
    {
        private readonly ILogger logger = Log.ForContext<SeasonRatingDeleteHandler>();
        private readonly ISeasonRatingRead seasonRatingReadRepo;
        private readonly ISeasonRatingWrite seasonRatingWriteRepo;

        public SeasonRatingDeleteHandler(ISeasonRatingRead seasonRatingReadRepo, ISeasonRatingWrite seasonRatingWriteRepo)
        {
            this.seasonRatingReadRepo = seasonRatingReadRepo;
            this.seasonRatingWriteRepo = seasonRatingWriteRepo;
        }

        public async Task DeleteSeasonRating(long id)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(id)}: [{id}].");

                if (id <= 0)
                {
                    var error = new ErrorModel(code: ErrorCodes.OutOfRangeProperty.GetIntValueAsString(), description: $"The given id [{id}] is not a valid id. A valid id must be greater than 0!",
                        source: nameof(id), title: ErrorCodes.OutOfRangeProperty.GetDescription());
                    throw new NotExistingIdException(error, $"The given {nameof(SeasonRating)}'s id is less than/equal to 0!");
                }

                var seasonRating = await seasonRatingReadRepo.GetSeasonRatingById(id);
                if (seasonRating == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                         description: $"No {nameof(SeasonRating)} object was found with the given id [{id}]!",
                         source: nameof(id), title: ErrorCodes.EmptyObject.GetDescription()
                     );
                    throw new NotFoundObjectException<SeasonRating>(error, $"Not found an {nameof(SeasonRating)} entity with id: [{id}].");
                }

                await seasonRatingWriteRepo.DeleteSeasonRating(seasonRating);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");
            }
            catch (NotExistingIdException noIdEx)
            {
                logger.Warning(noIdEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{noIdEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<SeasonRating> nfoEx)
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
