using GameRaitingAPI.Entitie;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameRaitingAPI
{
    public class AppDbContext : DbContext
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


        }

        public DbSet<Genre> genres { get; set; }
        public DbSet<Game> games { get; set; }
        public DbSet<GameGenres> gameGenres {get;set;}

    }
}
