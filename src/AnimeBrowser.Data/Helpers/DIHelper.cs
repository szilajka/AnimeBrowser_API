using AnimeBrowser.Data.Interfaces.Read.MainInterfaces;
using AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces;
using AnimeBrowser.Data.Interfaces.Write.MainInterfaces;
using AnimeBrowser.Data.Interfaces.Write.SecondaryInterfaces;
using AnimeBrowser.Data.Repositories.Read.MainRepositories;
using AnimeBrowser.Data.Repositories.Read.SecondaryRepositories;
using AnimeBrowser.Data.Repositories.Write.MainRepositories;
using AnimeBrowser.Data.Repositories.Write.SecondaryRepositories;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AnimeBrowser.Data.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class DIHelper
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            #region Main Repositories
            services.AddTransient<IAnimeInfoRead, AnimeInfoReadRepository>();
            services.AddTransient<IAnimeInfoWrite, AnimeInfoWriteRepository>();
            services.AddTransient<IGenreRead, GenreReadRepository>();
            services.AddTransient<IGenreWrite, GenreWriteRepository>();
            services.AddTransient<ISeasonRead, SeasonReadRepository>();
            services.AddTransient<ISeasonWrite, SeasonWriteRepository>();
            services.AddTransient<IEpisodeRead, EpisodeReadRepository>();
            services.AddTransient<IEpisodeWrite, EpisodeWriteRepository>();
            #endregion Main Repositories

            #region Secondary Repositories
            services.AddTransient<IAnimeInfoNameRead, AnimeInfoNameReadRepository>();
            services.AddTransient<IAnimeInfoNameWrite, AnimeInfoNameWriteRepository>();
            services.AddTransient<ISeasonNameRead, SeasonNameReadRepository>();
            services.AddTransient<ISeasonNameWrite, SeasonNameWriteRepository>();
            services.AddTransient<ISeasonGenreRead, SeasonGenreReadRepository>();
            services.AddTransient<ISeasonGenreWrite, SeasonGenreWriteRepository>();
            #endregion Secondary Repositories
        }
    }
}
