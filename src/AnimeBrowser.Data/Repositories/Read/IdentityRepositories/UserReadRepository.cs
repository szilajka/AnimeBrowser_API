using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Data.Entities;
using AnimeBrowser.Data.Entities.Identity;
using AnimeBrowser.Data.Interfaces.Read.IdentityInterfaces;
using Serilog;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Repositories.Read.IdentityRepositories
{
    public class UserReadRepository : IUserRead
    {
        private readonly AnimeBrowserContext abContext;
        private readonly ILogger logger = Log.ForContext<UserReadRepository>();

        public UserReadRepository(AnimeBrowserContext abContext)
        {
            this.abContext = abContext;
        }

        public async Task<User?> GetUserById(string userId)
        {
            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method started. {nameof(userId)}: [{userId}].");

            var user = await abContext.Users.FindAsync(userId);

            logger.Debug($"[{MethodNameHelper.GetCurrentMethodName()}] method finished. Found {nameof(user)}.{nameof(user.Id)}: [{user?.Id}].");
            return user;
        }
    }
}
