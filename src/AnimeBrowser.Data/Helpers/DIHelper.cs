using AnimeBrowser.Data.Interfaces.Write;
using AnimeBrowser.Data.Repositories.Write;
using Microsoft.Extensions.DependencyInjection;

namespace AnimeBrowser.Data.Helpers
{
    public static class DIHelper
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IAnimeInfoWrite, AnimeInfoWriteRepository>();
        }
    }
}
