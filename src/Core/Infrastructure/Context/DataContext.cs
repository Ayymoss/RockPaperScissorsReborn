using Microsoft.EntityFrameworkCore;
using RockPaperScissors.Core.Domain.Entities;

namespace RockPaperScissors.Core.Infrastructure.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<EFLeaderboard> Leaderboards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EFLeaderboard>().ToTable("EFLeaderboards");

        var randomUserOne = new EFLeaderboard
        {
            Id = -1,
            UserName = "Jeff",
            Guid = Guid.NewGuid(),
            Streak = 12,
            Duration = TimeSpan.FromMinutes(12),
            Submitted = DateTimeOffset.UtcNow
        };

        var randomUserTwo = new EFLeaderboard
        {
            Id = -2,
            UserName = "Dave",
            Guid = Guid.NewGuid(),
            Streak = 142,
            Duration = TimeSpan.FromMinutes(42),
            Submitted = DateTimeOffset.UtcNow.AddHours(-6)
        };

        var randomUserThree = new EFLeaderboard
        {
            Id = -3,
            UserName = "Jane",
            Guid = Guid.NewGuid(),
            Streak = 26,
            Duration = TimeSpan.FromDays(3),
            Submitted = DateTimeOffset.UtcNow.AddDays(-2)
        };

        modelBuilder.Entity<EFLeaderboard>().HasData(randomUserOne);
        modelBuilder.Entity<EFLeaderboard>().HasData(randomUserTwo);
        modelBuilder.Entity<EFLeaderboard>().HasData(randomUserThree);

        base.OnModelCreating(modelBuilder);
    }
}
