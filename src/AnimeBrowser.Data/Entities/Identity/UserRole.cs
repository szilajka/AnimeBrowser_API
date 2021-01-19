#nullable disable

namespace AnimeBrowser.Data.Entities.Identity
{
    public partial class UserRole
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
