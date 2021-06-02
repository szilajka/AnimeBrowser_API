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

            services.AddTransient<IAnimeInfoActivation, AnimeInfoActivationHandler>();
            services.AddTransient<IAnimeInfoCreation, AnimeInfoCreationHandler>();
            services.AddTransient<IAnimeInfoDelete, AnimeInfoDeleteHandler>();
            services.AddTransient<IAnimeInfoEditing, AnimeInfoEditingHandler>();
            services.AddTransient<IAnimeInfoInactivation, AnimeInfoInactivationHandler>();

            services.AddTransient<IEpisodeActivation, EpisodeActivationHandler>();
            services.AddTransient<IEpisodeCreation, EpisodeCreationHandler>();
            services.AddTransient<IEpisodeDelete, EpisodeDeleteHandler>();
            services.AddTransient<IEpisodeEditing, EpisodeEditingHandler>();
            services.AddTransient<IEpisodeInactivation, EpisodeInactivationHandler>();

            services.AddTransient<IGenreCreation, GenreCreationHandler>();
            services.AddTransient<IGenreDelete, GenreDeleteHandler>();
            services.AddTransient<IGenreEditing, GenreEditingHandler>();

            services.AddTransient<ISeasonActivation, SeasonActivationHandler>();
            services.AddTransient<ISeasonCreation, SeasonCreationHandler>();
            services.AddTransient<ISeasonDelete, SeasonDeleteHandler>();
            services.AddTransient<ISeasonEditing, SeasonEditingHandler>();
            services.AddTransient<ISeasonInactivation, SeasonInactivationHandler>();

            #endregion Main Handlers

            #region Secondary Handlers
            services.AddTransient<IAnimeInfoNameCreation, AnimeInfoNameCreationHandler>();
            services.AddTransient<IAnimeInfoNameDelete, AnimeInfoNameDeleteHandler>();
            services.AddTransient<IAnimeInfoNameEditing, AnimeInfoNameEditingHandler>();

            services.AddTransient<IEpisodeRatingCreation, EpisodeRatingCreationHandler>();
            services.AddTransient<IEpisodeRatingDelete, EpisodeRatingDeleteHandler>();
            services.AddTransient<IEpisodeRatingEditing, EpisodeRatingEditingHandler>();

            services.AddTransient<ISeasonGenreCreation, SeasonGenreCreationHandler>();
            services.AddTransient<ISeasonGenreDelete, SeasonGenreDeleteHandler>();

            services.AddTransient<ISeasonNameCreation, SeasonNameCreationHandler>();
            services.AddTransient<ISeasonNameDelete, SeasonNameDeleteHandler>();
            services.AddTransient<ISeasonNameEditing, SeasonNameEditingHandler>();

            services.AddTransient<ISeasonRatingCreation, SeasonRatingCreationHandler>();
            services.AddTransient<ISeasonRatingDelete, SeasonRatingDeleteHandler>();
            services.AddTransient<ISeasonRatingEditing, SeasonRatingEditingHandler>();
            #endregion Secondary Handlers
        }
    }
}
