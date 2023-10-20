using Microsoft.EntityFrameworkCore;
using RockPaperScissors.Core.Business.DTOs;
using RockPaperScissors.Core.Business.Mediatr.Commands;
using RockPaperScissors.Core.Domain.Interfaces;
using RockPaperScissors.Core.Domain.ValueObjects.Pagination;
using RockPaperScissors.Core.Infrastructure.Context;
using RockPaperScissors.Core.Infrastructure.Utilities;

namespace RockPaperScissors.Core.Infrastructure.Repositories.Pagination;

public class LeaderboardPaginationQueryHelper(IDbContextFactory<DataContext> contextFactory) : IResourceQueryHelper<GetLeaderboardCommand, Leaderboard>
{
    public async Task<PaginationContext<Leaderboard>> QueryResourceAsync(GetLeaderboardCommand request,
        CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var query = context.Leaderboards.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchString))
            query = query.Where(search => EF.Functions.ILike(search.UserName, $"%{request.SearchString}%"));

        if (request.Sorts.Any())
            query = request.Sorts.Aggregate(query, (current, sort) => sort.Property switch
            {
                "UserName" => current.ApplySort(sort, p => p.UserName),
                "Streak" => current.ApplySort(sort, p => p.Streak),
                "Duration" => current.ApplySort(sort, p => p.Duration ?? TimeSpan.Zero),
                "Submitted" => current.ApplySort(sort, p => p.Submitted ?? DateTimeOffset.UtcNow),
                _ => current
            });

        var count = await query.CountAsync(cancellationToken: cancellationToken);

        var pagedData = await query
            .Skip(request.Skip)
            .Take(request.Top)
            .Select(q => new Leaderboard
            {
                UserName = q.UserName,
                Streak = q.Streak,
                Duration = q.Duration ?? TimeSpan.Zero,
                Submitted = q.Submitted ?? DateTimeOffset.UtcNow
            }).ToListAsync(cancellationToken: cancellationToken);

        return new PaginationContext<Leaderboard>
        {
            Data = pagedData,
            Count = count
        };
    }
}
