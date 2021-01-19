using System;
using System.Collections.Generic;

#nullable disable

namespace AnimeBrowser.Data.Entities.Identity
{
    public partial class User
    {
        public User()
        {
            AnimeEpisodeRatings = new HashSet<AnimeEpisodeRating>();
            AnimeLists = new HashSet<AnimeList>();
            AnimeRatings = new HashSet<AnimeRating>();
            UserClaims = new HashSet<UserClaim>();
            UserLogins = new HashSet<UserLogin>();
            UserRoles = new HashSet<UserRole>();
            UserTokens = new HashSet<UserToken>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public virtual ICollection<AnimeEpisodeRating> AnimeEpisodeRatings { get; set; }
        public virtual ICollection<AnimeList> AnimeLists { get; set; }
        public virtual ICollection<AnimeRating> AnimeRatings { get; set; }
        public virtual ICollection<UserClaim> UserClaims { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserToken> UserTokens { get; set; }
    }
}
