using AnimeBrowser.Data.Interfaces.Read;
using AnimeBrowser.Data.Interfaces.Write;
using AnimeBrowser.Data.Repositories.Read;
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
            services.AddTransient<IAnimeInfoRead, AnimeInfoReadRepository>();
            services.AddTransient<IAnimeInfoWrite, AnimeInfoWriteRepository>();
            services.AddTransient<IGenreRead, GenreReadRepository>();
            services.AddTransient<IGenreWrite, GenreWriteRepository>();
            services.AddTransient<ISeasonRead, SeasonReadRepository>();
            services.AddTransient<ISeasonWrite, SeasonWriteRepository>();
        }
    }
}
