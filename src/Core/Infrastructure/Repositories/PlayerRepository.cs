using Microsoft.EntityFrameworkCore;
using RockPaperScissors.Core.Domain.Entities;
using RockPaperScissors.Core.Domain.Interfaces.Repositories;
using RockPaperScissors.Core.Domain.ValueObjects;
using RockPaperScissors.Core.Infrastructure.Context;

namespace RockPaperScissors.Core.Infrastructure.Repositories;

public class PlayerRepository(IDbContextFactory<DataContext> contextFactory) : IPlayerRepository
{
    public async Task<EFPlayer?> GetPlayerDataAsync(Guid guid, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var player = await context.Players
            .Where(p => p.Guid == guid)
            .Include(p => p.Leaderboard)
            .FirstOrDefaultAsync(cancellationToken);
        return player;
    }

    public async Task<int> CreatePlayerAsync(EFPlayer player, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        context.Players.Add(player);
        await context.SaveChangesAsync(cancellationToken);
        return player.Id;
    }
}
