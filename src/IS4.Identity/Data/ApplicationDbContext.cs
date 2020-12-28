using IS4Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IS4Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<IdentityRole>(b => b.ToTable("Roles"));
            builder.Entity<IdentityRoleClaim<string>>(b => b.ToTable("RoleClaims"));
            builder.Entity<IdentityUserRole<string>>(b => b.ToTable("UserRoles"));

            builder.Entity<ApplicationUser>(b => b.ToTable("Users"));
            builder.Entity<IdentityUserLogin<string>>(b => b.ToTable("UserLogins"));
            builder.Entity<IdentityUserClaim<string>>(b => b.ToTable("UserClaims"));
            builder.Entity<IdentityUserToken<string>>(b => b.ToTable("UserTokens"));
        }
    }
}
