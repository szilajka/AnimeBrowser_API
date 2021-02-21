using AnimeBrowser.BL.Interfaces.Read;
using AnimeBrowser.BL.Interfaces.Write;
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
            services.AddTransient<IAnimeInfoItemReader, AnimeInfoItemReader>();
            services.AddTransient<IAnimeInfoCreation, AnimeInfoCreationHandler>();
            services.AddTransient<IAnimeInfoEditing, AnimeInfoEditingHandler>();
            services.AddTransient<IAnimeInfoDelete, AnimeInfoDeleteHandler>();
            services.AddTransient<IGenreCreation, GenreCreationHandler>();
            services.AddTransient<IGenreEditing, GenreEditingHandler>();
            services.AddTransient<IGenreDelete, GenreDeleteHandler>();
        }
    }
}
