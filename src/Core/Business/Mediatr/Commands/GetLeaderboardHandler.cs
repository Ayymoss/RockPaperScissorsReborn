using MediatR;
using RockPaperScissors.Core.Business.DTOs;
using RockPaperScissors.Core.Domain.Interfaces;
using RockPaperScissors.Core.Domain.ValueObjects.Pagination;

namespace RockPaperScissors.Core.Business.Mediatr.Commands;

public class GetLeaderboardHandler(IResourceQueryHelper<GetLeaderboardCommand, Leaderboard> resourceQueryHelper)
    : IRequestHandler<GetLeaderboardCommand, PaginationContext<Leaderboard>>
{
    public async Task<PaginationContext<Leaderboard>> Handle(GetLeaderboardCommand request, CancellationToken cancellationToken)
    {
        var result = await resourceQueryHelper.QueryResourceAsync(request, cancellationToken);
        return result;
    }
}
