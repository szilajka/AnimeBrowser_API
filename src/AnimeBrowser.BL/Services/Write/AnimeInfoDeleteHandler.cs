using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.Common.Exceptions;
using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read;
using AnimeBrowser.Data.Interfaces.Write;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AnimeBrowser.BL.Services.Write
{
    public class AnimeInfoDeleteHandler : IAnimeInfoDelete
    {
        private readonly ILogger logger = Log.ForContext<AnimeInfoDeleteHandler>();
        private readonly IAnimeInfoRead animeInfoReadRepo;
        private readonly IAnimeInfoWrite animeInfoWriteRepo;

        public AnimeInfoDeleteHandler(IAnimeInfoRead animeInfoReadRepo, IAnimeInfoWrite animeInfoWriteRepo)
        {
            this.animeInfoReadRepo = animeInfoReadRepo;
            this.animeInfoWriteRepo = animeInfoWriteRepo;
        }

        public async Task DeleteAnimeInfo(long id)
        {
            try
            {
                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method started. anime info's id: [{id}].");

                if (id <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(id), "The given anime info's id is less than/equal to 0!");
                }

                var animeInfo = await animeInfoReadRepo.GetAnimeInfoById(id);
                if (animeInfo == null)
                {
                    throw new NotFoundObjectException<AnimeInfo>($"Not found an anime entity with id: [{id}].");
                }

                await animeInfoWriteRepo.DeleteAnimeInfo(animeInfo);

                logger.Information($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Successfully deleted anime info [{id}].");
            }
            catch (NotFoundObjectException<AnimeInfo> nfoEx)
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
