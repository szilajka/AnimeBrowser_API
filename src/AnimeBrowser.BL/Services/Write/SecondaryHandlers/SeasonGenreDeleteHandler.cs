using AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces;
using AnimeBrowser.Data.Interfaces.Write.SecondaryInterfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write.SecondaryHandlers
{
    public class SeasonGenreDeleteHandler : ISeasonGenreDelete
    {
        private readonly ILogger logger = Log.ForContext<SeasonGenreDeleteHandler>();
        private readonly ISeasonGenreRead seasonGenreReadRepo;
        private readonly ISeasonGenreWrite seasonGenreWriteRepo;

        public SeasonGenreDeleteHandler(ISeasonGenreRead seasonGenreReadRepo, ISeasonGenreWrite seasonGenreWriteRepo)
        {
            this.seasonGenreReadRepo = seasonGenreReadRepo;
            this.seasonGenreWriteRepo = seasonGenreWriteRepo;
        }

        public async Task DeleteSeasonGenres(IList<long> seasonGenreIds)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(seasonGenreIds)}: [{string.Join(", ", seasonGenreIds ?? new List<long>())}].");
                if (seasonGenreIds == null || seasonGenreIds.Any(id => id <= 0))
                {
                    ErrorModel error;
                    if (seasonGenreIds != null)
                    {
                        var wrongIds = seasonGenreIds.Where(id => id <= 0);
                        error = new ErrorModel(code: ErrorCodes.OutOfRangeProperty.GetIntValueAsString(), description: $"The given ids [{string.Join(", ", wrongIds)}] are not valid ids. A valid id must be greater than 0!",
                            source: nameof(seasonGenreIds), title: ErrorCodes.OutOfRangeProperty.GetDescription());
                    }
                    else
                    {
                        error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(), description: $"The given object [{nameof(seasonGenreIds)}] is an empty object!",
                            source: nameof(seasonGenreIds), title: ErrorCodes.EmptyObject.GetDescription());
                    }
                    throw new NotExistingIdException(error, $"The given {nameof(SeasonGenre)}'s ids are less than/equal to 0!");
                }

                var seasonGenres = seasonGenreReadRepo.GetSeasonGenresByIds(seasonGenreIds);
                if (seasonGenres == null || !seasonGenres.Any())
                {
                    var error = new ErrorModel(code: ErrorCodes.EmptyObject.GetIntValueAsString(),
                         description: $"No {nameof(SeasonGenre)} object was found with given ids [{string.Join(", ", seasonGenreIds)}]!",
                         source: nameof(seasonGenreIds), title: ErrorCodes.EmptyObject.GetDescription()
                     );
                    throw new NotFoundObjectException<SeasonGenre>(error, $"Not found any {nameof(SeasonGenre)} entity with ids: [{string.Join(", ", seasonGenreIds)}].");
                }

                await seasonGenreWriteRepo.DeleteSeasonGenres(seasonGenres);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished.");
            }
            catch (NotExistingIdException noIdEx)
            {
                logger.Warning(noIdEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{noIdEx.Message}].");
                throw;
            }
            catch (NotFoundObjectException<SeasonGenre> nfoEx)
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
