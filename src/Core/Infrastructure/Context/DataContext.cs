using Microsoft.EntityFrameworkCore;
using RockPaperScissors.Core.Domain.Entities;

namespace RockPaperScissors.Core.Infrastructure.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<EFPlayer> Players { get; set; }
    public DbSet<EFLeaderboard> Leaderboards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EFPlayer>().ToTable("EFPlayers");
        modelBuilder.Entity<EFLeaderboard>().ToTable("EFLeaderboards");

        var randomPlayerOne = new EFPlayer
        {
            Id = -1,
            Guid = Guid.NewGuid(),
            UserName = "Jeff",
            Chips = 100,
        };
        var randomPlayerTwo = new EFPlayer
        {
            Id = -2,
            Guid = Guid.NewGuid(),
            UserName = "Dave",
            Chips = 100,
        };

        modelBuilder.Entity<EFPlayer>().HasData(randomPlayerOne);
        modelBuilder.Entity<EFPlayer>().HasData(randomPlayerTwo);

        var randomUserOne = new EFLeaderboard
        {
            Id = -1,
            PlayerId = randomPlayerOne.Id,
            Streak = 12,
            Duration = TimeSpan.FromMinutes(12),
            Submitted = DateTimeOffset.UtcNow
        };

        var randomUserTwo = new EFLeaderboard
        {
            Id = -2,
            PlayerId = randomPlayerTwo.Id,
            Streak = 142,
            Duration = TimeSpan.FromMinutes(42),
            Submitted = DateTimeOffset.UtcNow.AddHours(-6)
        };

        modelBuilder.Entity<EFLeaderboard>().HasData(randomUserOne);
        modelBuilder.Entity<EFLeaderboard>().HasData(randomUserTwo);

        base.OnModelCreating(modelBuilder);
    }
}
