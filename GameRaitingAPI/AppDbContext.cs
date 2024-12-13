using GameRaitingAPI.Entitie;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameRaitingAPI
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().Property(g => g.Name).HasMaxLength(50);
            modelBuilder.Entity<Game>().Property(g => g.Name).HasMaxLength(50);
            modelBuilder.Entity<Game>().Property(g => g.Photo).IsUnicode();

            modelBuilder.Entity<GameGenres>().HasKey(g => new { g.GenreId, g.GameId });

            modelBuilder.Entity<Rating>()
                .Property(r => r.Stars)
                .HasAnnotation("MinValue", 1)
                .HasAnnotation("MaxValue", 5);

            modelBuilder.Entity<IdentityUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

        }

        public DbSet<Genre> genres { get; set; }
        public DbSet<Game> games { get; set; }
        public DbSet<GameGenres> gameGenres { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Rating> ratings { get; set; }

    }
}
