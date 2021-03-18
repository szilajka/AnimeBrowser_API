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
    public class AnimeInfoNameDeleteHandler : IAnimeInfoNameDelete
    {
        private readonly ILogger logger = Log.ForContext<AnimeInfoNameDeleteHandler>();
        private readonly IAnimeInfoNameRead animeInfoNameReadRepo;
        private readonly IAnimeInfoNameWrite animeInfoNameWriteRepo;

        public AnimeInfoNameDeleteHandler(IAnimeInfoNameRead animeInfoNameReadRepo, IAnimeInfoNameWrite animeInfoNameWriteRepo)
        {
            this.animeInfoNameReadRepo = animeInfoNameReadRepo;
            this.animeInfoNameWriteRepo = animeInfoNameWriteRepo;
        }

        public async Task DeleteAnimeInfoName(long id)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(id)}: [{id}].");

                if (id <= 0)
                {
                    var error = new ErrorModel(code: ErrorCodes.OutOfRangeProperty.GetIntValueAsString(), description: $"The given id [{id}] is not a valid id. A valid id must be greater than 0!",
                        source: nameof(id), title: ErrorCodes.OutOfRangeProperty.GetDescription());
                    throw new NotExistingIdException(error, $"The given {nameof(AnimeInfoName)}'s id is less than/equal to 0!");
                }

                var animeInfoName = await animeInfoNameReadRepo.GetAnimeInfoNameById(id);
                if (animeInfoName == null)
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                         description: $"No {nameof(AnimeInfoName)} object was found with the given id [{id}]!",
                         source: nameof(id), title: ErrorCodes.EmptyObject.GetDescription()
                     );
                    throw new NotFoundObjectException<AnimeInfoName>(error, $"Not found an {nameof(AnimeInfoName)} entity with id: [{id}].");
                }

                await animeInfoNameWriteRepo.DeleteAnimeInfoName(animeInfoName);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");
            }
            catch (NotExistingIdException noIdEx)
            {
                logger.Warning(noIdEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{noIdEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<AnimeInfoName> nfoEx)
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
