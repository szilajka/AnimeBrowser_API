using AnimeBrowser.Data.Entities.Identity;
using System.Threading.Tasks;

namespace AnimeBrowser.Data.Interfaces.Read.IdentityInterfaces
{
    public interface IUserRead
    {
        Task<User?> GetUserById(string userId);
    }
}
