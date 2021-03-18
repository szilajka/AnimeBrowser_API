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
    public class SeasonNameDeleteHandler : ISeasonNameDelete
    {
        private readonly ILogger logger = Log.ForContext<SeasonNameDeleteHandler>();
        private readonly ISeasonNameRead seasonNameReadRepo;
        private readonly ISeasonNameWrite seasonNameWriteRepo;

        public SeasonNameDeleteHandler(ISeasonNameRead seasonNameReadRepo, ISeasonNameWrite seasonNameWriteRepo)
        {
            this.seasonNameReadRepo = seasonNameReadRepo;
            this.seasonNameWriteRepo = seasonNameWriteRepo;
        }

        public async Task DeleteSeasonName(long id)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(id)}: [{id}].");

                if (id <= 0)
                {
                    var error = new ErrorModel(code: ErrorCodes.OutOfRangeProperty.GetIntValueAsString(), description: $"The given id [{id}] is not a valid id. A valid id must be greater than 0!",
                        source: nameof(id), title: ErrorCodes.OutOfRangeProperty.GetDescription());
                    throw new NotExistingIdException(error, $"The given {nameof(SeasonName)}'s id is less than/equal to 0!");
                }

                var seasonName = await seasonNameReadRepo.GetSeasonNameById(id);
                if (seasonName == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                         description: $"No {nameof(SeasonName)} object was found with the given id [{id}]!",
                         source: nameof(id), title: ErrorCodes.EmptyObject.GetDescription()
                     );
                    throw new NotFoundObjectException<SeasonName>(error, $"Not found an {nameof(SeasonName)} entity with id: [{id}].");
                }

                await seasonNameWriteRepo.DeleteSeasonName(seasonName);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");
            }
            catch (NotExistingIdException noIdEx)
            {
                logger.Warning(noIdEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{noIdEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<SeasonName> nfoEx)
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
