using AnimeBrowser.Data.Entities.Identity;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AnimeBrowser.Data.Entities
{
    public partial class AnimeBrowserContext : DbContext
    {
        public AnimeBrowserContext()
        {
        }

        public AnimeBrowserContext(DbContextOptions<AnimeBrowserContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AnimeInfo> AnimeInfos { get; set; }
        public virtual DbSet<AnimeInfoName> AnimeInfoNames { get; set; }
        public virtual DbSet<Episode> Episodes { get; set; }
        public virtual DbSet<EpisodeMediaList> EpisodeMediaLists { get; set; }
        public virtual DbSet<EpisodeRating> EpisodeRatings { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<MediaList> MediaLists { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleClaim> RoleClaims { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }
        public virtual DbSet<SeasonGenre> SeasonGenres { get; set; }
        public virtual DbSet<SeasonMediaList> SeasonMediaLists { get; set; }
        public virtual DbSet<SeasonName> SeasonNames { get; set; }
        public virtual DbSet<SeasonRating> SeasonRatings { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserClaim> UserClaims { get; set; }
        public virtual DbSet<UserLogin> UserLogins { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Hungarian_Hungary.1250");

            modelBuilder.Entity<AnimeInfo>(entity =>
            {
                entity.ToTable("anime_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasColumnType("character varying")
                    .HasColumnName("description");

                entity.Property(e => e.IsNsfw).HasColumnName("is_nsfw");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<AnimeInfoName>(entity =>
            {
                entity.ToTable("anime_info_name");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AnimeInfoId).HasColumnName("anime_info_id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.HasOne(d => d.AnimeInfo)
                    .WithMany(p => p.AnimeInfoNames)
                    .HasForeignKey(d => d.AnimeInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_anime_info_name_anime_info_id");
            });

            modelBuilder.Entity<Episode>(entity =>
            {
                entity.ToTable("episode");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AirDate)
                    .HasColumnType("date")
                    .HasColumnName("air_date");

                entity.Property(e => e.AirStatus).HasColumnName("air_status");

                entity.Property(e => e.AnimeInfoId).HasColumnName("anime_info_id");

                entity.Property(e => e.Cover).HasColumnName("cover");

                entity.Property(e => e.Description)
                    .HasColumnType("character varying")
                    .HasColumnName("description");

                entity.Property(e => e.EpisodeNumber).HasColumnName("episode_number");

                entity.Property(e => e.Rating)
                    .HasPrecision(3, 2)
                    .HasColumnName("rating");

                entity.Property(e => e.SeasonId).HasColumnName("season_id");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.HasOne(d => d.AnimeInfo)
                    .WithMany(p => p.Episodes)
                    .HasForeignKey(d => d.AnimeInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_episode_anime_info_id");

                entity.HasOne(d => d.Season)
                    .WithMany(p => p.Episodes)
                    .HasForeignKey(d => d.SeasonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_episode_season_id");
            });

            modelBuilder.Entity<EpisodeMediaList>(entity =>
            {
                entity.ToTable("episode_media_list");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EpisodeId).HasColumnName("episode_id");

                entity.Property(e => e.ListId).HasColumnName("list_id");

                entity.HasOne(d => d.Episode)
                    .WithMany(p => p.EpisodeMediaLists)
                    .HasForeignKey(d => d.EpisodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_episode_media_list_episode_id");

                entity.HasOne(d => d.List)
                    .WithMany(p => p.EpisodeMediaLists)
                    .HasForeignKey(d => d.ListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_episode_media_list_list_id");
            });

            modelBuilder.Entity<EpisodeRating>(entity =>
            {
                entity.ToTable("episode_rating");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EpisodeId).HasColumnName("episode_id");

                entity.Property(e => e.Message)
                    .HasMaxLength(30000)
                    .HasColumnName("message");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Episode)
                    .WithMany(p => p.EpisodeRatings)
                    .HasForeignKey(d => d.EpisodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_episode_rating_episode_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EpisodeRatings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_episode_rating_user_id");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genre");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("description");

                entity.Property(e => e.GenreName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("genre_name");
            });

            modelBuilder.Entity<MediaList>(entity =>
            {
                entity.ToTable("media_list");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsPublic).HasColumnName("is_public");

                entity.Property(e => e.ListType).HasColumnName("list_type");

                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .HasColumnName("name");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MediaLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_media_list_user_id");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles", "identity");

                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<RoleClaim>(entity =>
            {
                entity.ToTable("RoleClaims", "identity");

                entity.HasIndex(e => e.RoleId, "IX_RoleClaims_RoleId");

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<Season>(entity =>
            {
                entity.ToTable("season");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AirStatus).HasColumnName("air_status");

                entity.Property(e => e.AnimeInfoId).HasColumnName("anime_info_id");

                entity.Property(e => e.Cover).HasColumnName("cover");

                entity.Property(e => e.CoverCarousel).HasColumnName("cover_carousel");

                entity.Property(e => e.Description)
                    .HasColumnType("character varying")
                    .HasColumnName("description");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");

                entity.Property(e => e.NumberOfEpisodes).HasColumnName("number_of_episodes");

                entity.Property(e => e.Rating)
                    .HasPrecision(3, 2)
                    .HasColumnName("rating");

                entity.Property(e => e.SeasonNumber).HasColumnName("season_number");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.HasOne(d => d.AnimeInfo)
                    .WithMany(p => p.Seasons)
                    .HasForeignKey(d => d.AnimeInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_season_anime_info_id");
            });

            modelBuilder.Entity<SeasonGenre>(entity =>
            {
                entity.ToTable("season_genre");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GenreId).HasColumnName("genre_id");

                entity.Property(e => e.SeasonId).HasColumnName("season_id");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.SeasonGenres)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_season_genre_genre_id");

                entity.HasOne(d => d.Season)
                    .WithMany(p => p.SeasonGenres)
                    .HasForeignKey(d => d.SeasonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_season_genre_season_id");
            });

            modelBuilder.Entity<SeasonMediaList>(entity =>
            {
                entity.ToTable("season_media_list");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ListId).HasColumnName("list_id");

                entity.Property(e => e.SeasonId).HasColumnName("season_id");

                entity.HasOne(d => d.List)
                    .WithMany(p => p.SeasonMediaLists)
                    .HasForeignKey(d => d.ListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_season_media_list_list_id");

                entity.HasOne(d => d.Season)
                    .WithMany(p => p.SeasonMediaLists)
                    .HasForeignKey(d => d.SeasonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_season_media_list_season_id");
            });

            modelBuilder.Entity<SeasonName>(entity =>
            {
                entity.ToTable("season_name");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.SeasonId).HasColumnName("season_id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.HasOne(d => d.Season)
                    .WithMany(p => p.SeasonNames)
                    .HasForeignKey(d => d.SeasonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_season_name_season_id");
            });

            modelBuilder.Entity<SeasonRating>(entity =>
            {
                entity.ToTable("season_rating");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Message)
                    .HasMaxLength(30000)
                    .HasColumnName("message");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.SeasonId).HasColumnName("season_id");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Season)
                    .WithMany(p => p.SeasonRatings)
                    .HasForeignKey(d => d.SeasonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_season_rating_season_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SeasonRatings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_season_rating_user_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users", "identity");

                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.LockoutEnd).HasColumnType("timestamp with time zone");

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<UserClaim>(entity =>
            {
                entity.ToTable("UserClaims", "identity");

                entity.HasIndex(e => e.UserId, "IX_UserClaims_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.ToTable("UserLogins", "identity");

                entity.HasIndex(e => e.UserId, "IX_UserLogins_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.ToTable("UserRoles", "identity");

                entity.HasIndex(e => e.RoleId, "IX_UserRoles_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.ToTable("UserTokens", "identity");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
