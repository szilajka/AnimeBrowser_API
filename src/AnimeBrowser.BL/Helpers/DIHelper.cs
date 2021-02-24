using AnimeBrowser.BL.Interfaces.DateTimeProviders;
using AnimeBrowser.BL.Interfaces.Read;
using AnimeBrowser.BL.Interfaces.Write;
using AnimeBrowser.BL.Services.DateTimeProviders;
using AnimeBrowser.BL.Services.Read;
using AnimeBrowser.BL.Services.Write;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AnimeBrowser.BL.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class DIHelper
    {
        public static void AddBlHandlers(this IServiceCollection services)
        {
            services.AddSingleton<IDateTime, DateTimeProvider>();
            services.AddTransient<IAnimeInfoItemReader, AnimeInfoItemReader>();
            services.AddTransient<IAnimeInfoCreation, AnimeInfoCreationHandler>();
            services.AddTransient<IAnimeInfoEditing, AnimeInfoEditingHandler>();
            services.AddTransient<IAnimeInfoDelete, AnimeInfoDeleteHandler>();
            services.AddTransient<IGenreCreation, GenreCreationHandler>();
            services.AddTransient<IGenreEditing, GenreEditingHandler>();
            services.AddTransient<IGenreDelete, GenreDeleteHandler>();
            services.AddTransient<ISeasonCreation, SeasonCreationHandler>();
            services.AddTransient<ISeasonEditing, SeasonEditingHandler>();
            services.AddTransient<ISeasonDelete, SeasonDeleteHandler>();
            services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            services.AddTransient<IEpisodeEditing, EpisodeEditingHandler>();
            services.AddTransient<IEpisodeDelete, EpisodeDeleteHandler>();
        }
    }
}
