using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read;
using AnimeBrowser.Data.Interfaces.Write;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write
{
    public class SeasonDeleteHandler : ISeasonDelete
    {
        private readonly ILogger logger = Log.ForContext<SeasonDeleteHandler>();
        private readonly ISeasonWrite seasonWriteRepo;
        private readonly ISeasonRead seasonReadRepo;

        public SeasonDeleteHandler(ISeasonWrite seasonWriteRepo, ISeasonRead seasonReadRepo)
        {
            this.seasonWriteRepo = seasonWriteRepo;
            this.seasonReadRepo = seasonReadRepo;
        }

        public async Task DeleteSeason(long seasonId)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started with {nameof(seasonId)}: [{seasonId}].");

                if (seasonId <= 0)
                {
                    var error = new ErrorModel(code: ErrorCodes.OutOfRangeProperty.GetIntValueAsString(), description: $"The given id [{seasonId}] is not a valid id. A valid id must be greater than 0!",
                                          source: nameof(seasonId), title: ErrorCodes.OutOfRangeProperty.GetDescription());
                    throw new NotExistingIdException(error, $"The given {nameof(Season)}'s id is less than/equal to 0!");
                }

                var season = await seasonReadRepo.GetSeasonById(seasonId);
                if (season == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                      description: $"No {nameof(Season)} object was found with the given id [{seasonId}]!",
                      source: nameof(seasonId), title: ErrorCodes.EmptyObject.GetDescription()
                  );
                    throw new NotFoundObjectException<Season>(error, $"Not found a {nameof(Season)} entity with id: [{seasonId}].");
                }

                await seasonWriteRepo.DeleteSeason(season);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");
            }
            catch (NotExistingIdException noIdEx)
            {
                logger.Warning(noIdEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{noIdEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<Season> nfoEx)
            {
                logger.Warning(nfoEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{nfoEx.Message}].");
                throw;
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {MethodNameHelper.GetCurrentMethodName()}. Message: [{ex.Message}].");
                throw;
            }
        }
    }
}
