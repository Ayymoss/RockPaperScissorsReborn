using Microsoft.EntityFrameworkCore;
using RockPaperScissors.Core.Business.DTOs;
using RockPaperScissors.Core.Business.Mediatr.Commands;
using RockPaperScissors.Core.Domain.Interfaces;
using RockPaperScissors.Core.Domain.ValueObjects.Pagination;
using RockPaperScissors.Core.Infrastructure.Context;
using RockPaperScissors.Core.Infrastructure.Utilities;

namespace RockPaperScissors.Core.Infrastructure.Repositories.Pagination;

public class LeaderboardPaginationQueryHelper
    (IDbContextFactory<DataContext> contextFactory) : IResourceQueryHelper<GetLeaderboardCommand, Leaderboard>
{
    public async Task<PaginationContext<Leaderboard>> QueryResourceAsync(GetLeaderboardCommand request,
        CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var query = context.Leaderboards.Where(x => x.Streak > 1).AsQueryable();

        query = query.OrderByDescending(p => p.Streak).ThenBy(p => p.Duration ?? TimeSpan.Zero);

        var data = await query.Select(q => new
        {
            q.Player.UserName,
            q.Streak,
            q.Duration,
            q.Submitted
        }).ToListAsync(cancellationToken: cancellationToken);

        var rankedData = data.Select((item, index) => new Leaderboard
        {
            Rank = index + 1,
            UserName = item.UserName,
            Streak = item.Streak,
            Duration = item.Duration ?? TimeSpan.Zero,
            Submitted = item.Submitted ?? DateTimeOffset.UtcNow
        }).ToList();

        return new PaginationContext<Leaderboard>
        {
            Data = rankedData,
            Count = rankedData.Count
        };
    }
}
