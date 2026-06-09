using FootballApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Team> Teams { get; set; } = null!;
    public DbSet<PollVote> PollVotes { get; set; } = null!;
    public DbSet<TournamentState> TournamentStates { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(20);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CountryCode).IsRequired().HasMaxLength(3);
        });

        modelBuilder.Entity<PollVote>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.VotedAt).IsRequired();
            
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.Team)
                  .WithMany()
                  .HasForeignKey(e => e.TeamId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            // Ensure one vote per user at the database level too
            entity.HasIndex(e => e.UserId).IsUnique();
        });

        modelBuilder.Entity<TournamentState>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AreResultsRevealed).IsRequired();
        });

        // Seeding Data
        modelBuilder.Entity<TournamentState>().HasData(
            new TournamentState { Id = 1, AreResultsRevealed = false }
        );

        // Seed an Admin User
        // Note: PasswordHash should be properly hashed in a real application. 
        // We will seed an admin with a pre-hashed password or dummy value for demonstration.
        // Assuming BCrypt for password hashing: "admin123" -> "$2a$11$N/QO1T0Q0.b1p/n5I5A9iOg.V2cR8pZ1vN7.h4l9Vz2w1E5pZ8.C"
        modelBuilder.Entity<User>().HasData(
            new User 
            { 
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "Admin", 
                Email = "admin@example.com",
                PasswordHash = "$2b$10$jxnfsDgL9SGP7lV/vWJbPumnzTOEQ.SKexGQSdxWXL6lTkGb.YoTq", // Hash for "admin123"
                Role = "Admin"
            }
        );
    }
}
