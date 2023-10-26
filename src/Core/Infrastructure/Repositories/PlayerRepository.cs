using Microsoft.EntityFrameworkCore;
using RockPaperScissors.Core.Domain.Entities;
using RockPaperScissors.Core.Domain.Interfaces.Repositories;
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

    public async Task<int?> AdjustPlayerChipsAsync(Guid requestPlayerGuid, int requestAddChips, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var player = await context.Players.FirstOrDefaultAsync(p => p.Guid == requestPlayerGuid, cancellationToken);
        if (player is null) return null;

        player.Chips += requestAddChips;
        context.Players.Update(player);
        await context.SaveChangesAsync(cancellationToken);
        return player.Chips;
    }
}
