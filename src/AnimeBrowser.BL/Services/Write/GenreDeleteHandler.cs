using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read;
using AnimeBrowser.Data.Interfaces.Write;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write
{
    public class GenreDeleteHandler : IGenreDelete
    {
        private readonly ILogger logger = Log.ForContext<GenreEditingHandler>();
        private readonly IGenreRead genreReadRepo;
        private readonly IGenreWrite genreWriteRepo;

        public GenreDeleteHandler(IGenreRead genreRead, IGenreWrite genreWrite)
        {
            this.genreReadRepo = genreRead;
            this.genreWriteRepo = genreWrite;
        }

        public async Task DeleteGenre(long genreId)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. genreId: [{genreId}].");

                if (genreId <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(genreId), $"The given {nameof(Genre)}'s id is less than/equal to 0!");
                }

                var genre = await genreReadRepo.GetGenreById(genreId);
                if (genre == null)
                {
                    throw new NotFoundObjectException<Genre>($"Not found a {nameof(Genre)} entity with id: [{genreId}].");
                }

                await genreWriteRepo.DeleteGenre(genre);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Successfully deleted {nameof(Genre)} [{genreId}].");
            }
            catch (NotFoundObjectException<Genre> nfoEx)
            {
                logger.Warning(nfoEx, $"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{nfoEx.Message}].");
                throw;
            }
            catch (Exception ex)
            {
                logger.Error($"Error in [{MethodNameHelper.GetCurrentMethodName()}]. Message: [{ex.Message}].");
                throw;
            }
        }
    }
}
