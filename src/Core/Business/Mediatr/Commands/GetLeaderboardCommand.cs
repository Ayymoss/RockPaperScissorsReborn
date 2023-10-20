using MediatR;
using RockPaperScissors.Core.Business.DTOs;
using RockPaperScissors.Core.Domain.ValueObjects.Pagination;

namespace RockPaperScissors.Core.Business.Mediatr.Commands;

public class GetLeaderboardCommand : Pagination, IRequest<PaginationContext<Leaderboard>>
{
}
