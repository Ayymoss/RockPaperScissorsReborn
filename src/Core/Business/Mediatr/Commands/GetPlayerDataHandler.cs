using MediatR;
using RockPaperScissors.Core.Domain.Interfaces.Repositories;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Business.Mediatr.Commands;

public class GetPlayerDataHandler(ILeaderboardRepository leaderboardRepository) : IRequestHandler<GetPlayerDataCommand, Player?>
{
    public async Task<Player?> Handle(GetPlayerDataCommand request, CancellationToken cancellationToken)
    {
        var player = await leaderboardRepository.GetPlayerDataAsync(request.PlayerGuid, cancellationToken);
        return player;
    }
}
