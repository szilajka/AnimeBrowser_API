using AnimeBrowser.BL.Interfaces.DateTimeProviders;
using AnimeBrowser.BL.Interfaces.Read;
using AnimeBrowser.BL.Interfaces.Write.MainInterfaces;
using AnimeBrowser.BL.Interfaces.Write.SecondaryInterfaces;
using AnimeBrowser.BL.Services.DateTimeProviders;
using AnimeBrowser.BL.Services.Read;
using AnimeBrowser.BL.Services.Write.MainHandlers;
using AnimeBrowser.BL.Services.Write.SecondaryHandlers;
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
            #region Main Handlers
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
            #endregion Main Handlers

            #region Secondary Handlers
            services.AddTransient<IAnimeInfoNameCreation, AnimeInfoNameCreationHandler>();
            services.AddTransient<IAnimeInfoNameEditing, AnimeInfoNameEditingHandler>();
            services.AddTransient<IAnimeInfoNameDelete, AnimeInfoNameDeleteHandler>();
            services.AddTransient<ISeasonNameCreation, SeasonNameCreationHandler>();
            services.AddTransient<ISeasonNameEditing, SeasonNameEditingHandler>();
            services.AddTransient<ISeasonNameDelete, SeasonNameDeleteHandler>();
            services.AddTransient<ISeasonGenreCreation, SeasonGenreCreationHandler>();
            services.AddTransient<ISeasonGenreDelete, SeasonGenreDeleteHandler>();
            services.AddTransient<IEpisodeRatingCreation, EpisodeRatingCreationHandler>();
            services.AddTransient<IEpisodeRatingEditing, EpisodeRatingEditingHandler>();
            services.AddTransient<IEpisodeRatingDelete, EpisodeRatingDeleteHandler>();
            services.AddTransient<ISeasonRatingCreation, SeasonRatingCreationHandler>();
            #endregion Secondary Handlers
        }
    }
}
