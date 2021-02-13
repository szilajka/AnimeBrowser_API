using AnimeBrowser.BL.Interfaces.Read;
using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Services.Read;
using AnimeBrowser.BL.Services.Write;
using Microsoft.Extensions.DependencyInjection;

namespace AnimeBrowser.BL.Helpers
{
    public static class DIHelper
    {
        public static void AddBlHandlers(this IServiceCollection services)
        {
            services.AddTransient<IAnimeInfoItemReader, AnimeInfoItemReader>();
            services.AddTransient<IAnimeInfoCreation, AnimeInfoCreationHandler>();
        }
    }
}
