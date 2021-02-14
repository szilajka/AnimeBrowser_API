using AnimeBrowser.Data.Interfaces.Write;
using AnimeBrowser.Data.Repositories.Write;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AnimeBrowser.Data.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class DIHelper
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IAnimeInfoWrite, AnimeInfoWriteRepository>();
        }
    }
}
