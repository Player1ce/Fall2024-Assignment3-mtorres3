using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_mtorres3.Models;

namespace Fall2024_Assignment3_mtorres3.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>()
                .HasMany(a => a.Tweets)
                .WithOne(t => t.Actor)
                .HasForeignKey(t => t.ActorID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Movie>()
                .HasMany(a => a.Reviews)
                .WithOne(t => t.Movie)
                .HasForeignKey(t => t.MovieID)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Fall2024_Assignment3_mtorres3.Models.Movie> Movie { get; set; } = default!;
        public DbSet<Fall2024_Assignment3_mtorres3.Models.Actor> Actor { get; set; } = default!;
        public DbSet<Fall2024_Assignment3_mtorres3.Models.MovieActor> MovieActor { get; set; } = default!;
        public DbSet<Fall2024_Assignment3_mtorres3.Models.MovieReview> MovieReview { get; set; } = default!;
        public DbSet<Fall2024_Assignment3_mtorres3.Models.ActorTweet> ActorTweet { get; set; } = default!;
    }
}
