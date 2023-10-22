using Microsoft.EntityFrameworkCore;
using RockPaperScissors.Core.Domain.Entities;
using RockPaperScissors.Core.Domain.Interfaces.Repositories;
using RockPaperScissors.Core.Domain.ValueObjects;
using RockPaperScissors.Core.Infrastructure.Context;

namespace RockPaperScissors.Core.Infrastructure.Repositories;

public class LeaderboardRepository(IDbContextFactory<DataContext> contextFactory) : ILeaderboardRepository
{
    public async Task<int> GetPlayerStreakAsync(Guid guid, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var playerStreak = await context.Leaderboards
            .Where(p => p.Player.Guid == guid)
            .Select(p => p.Streak)
            .FirstOrDefaultAsync(cancellationToken);
        return playerStreak;
    }

    public async Task<int> GetPlayersWithBetterStreakAsync(int playerStreak, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var playersWithBetterStreak = await context.Leaderboards
            .CountAsync(p => p.Streak > playerStreak, cancellationToken);
        return playersWithBetterStreak;
    }

    public async Task<IEnumerable<EFLeaderboard>> GetPlayersWithSameStreakAsync(int playerStreak, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var sameStreakPlayers = await context.Leaderboards
            .Where(p => p.Streak == playerStreak)
            .Include(p => p.Player)
            .OrderBy(p => p.Duration)
            .ToListAsync(cancellationToken);
        return sameStreakPlayers;
    }

    public async Task<EFLeaderboard?> GetLeaderboardAsync(Guid guid, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var leaderboard = await context.Leaderboards
            .Where(p => p.Player.Guid == guid)
            .SingleOrDefaultAsync(cancellationToken);
        return leaderboard;
    }

    public async Task<int> AddOrUpdateLeaderboardAsync(EFLeaderboard leaderboard, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var efLeaderboard = await context.Leaderboards
            .Where(x => x.Id == leaderboard.Id)
            .AnyAsync(cancellationToken: cancellationToken);

        if (efLeaderboard) context.Leaderboards.Update(leaderboard);
        else context.Leaderboards.Add(leaderboard);

        await context.SaveChangesAsync(cancellationToken);
        return leaderboard.Id;
    }
}
