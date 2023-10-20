using Microsoft.EntityFrameworkCore;
using RockPaperScissors.Core.Domain.Entities;
using RockPaperScissors.Core.Domain.Interfaces.Repositories;
using RockPaperScissors.Core.Domain.ValueObjects;
using RockPaperScissors.Core.Infrastructure.Context;

namespace RockPaperScissors.Core.Infrastructure.Repositories;

public class LeaderboardRepository(IDbContextFactory<DataContext> contextFactory) : ILeaderboardRepository
{
    public async Task<Player?> GetPlayerDataAsync(Guid guid, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var player = await context.Leaderboards
            .Where(x => x.Guid == guid)
            .Select(x => new Player
            {
                UserName = x.UserName,
                Guid = guid,
                BestStreak = x.Streak,
            }).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        return player;
    }

    public async Task<int> CreatePlayerAsync(Player player, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var leaderboard = new EFLeaderboard
        {
            Id = 0,
            Guid = player.Guid,
            UserName = player.UserName,
            Streak = player.CurrentGame?.Streak ?? 0,
            Duration = player.CurrentGame?.Duration,
            Submitted = player.CurrentGame?.Submitted,
        };
        context.Leaderboards.Add(leaderboard);
        await context.SaveChangesAsync(cancellationToken);
        return leaderboard.Id;
    }

    public async Task UpdatePlayerAsync(Player player, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        if (player.CurrentGame is null) return;

        var leaderboard = await context.Leaderboards
            .Where(x => x.Guid == player.Guid)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (leaderboard is null) return;

        if (leaderboard.Streak < player.CurrentGame.Streak)
        {
            leaderboard.Streak = player.CurrentGame.Streak;
            leaderboard.Duration = player.CurrentGame.Duration;
            leaderboard.Submitted = player.CurrentGame.Submitted;
        }

        context.Leaderboards.Update(leaderboard);
        await context.SaveChangesAsync(cancellationToken);
    }
}
