using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Interfaces.Read.SecondaryInterfaces;
using Serilog;

namespace AnimeBrowser.Data.Repositories.Read.SecondaryRepositories
{
    public class AnimeInfoNameReadRepository : IAnimeInfoNameRead
    {
        private readonly ILogger logger = Log.ForContext<AnimeInfoNameReadRepository>();
        private readonly AnimeBrowserContext abContext;

        public AnimeInfoNameReadRepository(AnimeBrowserContext abContext)
        {
            this.abContext = abContext;
        }
    }
}
