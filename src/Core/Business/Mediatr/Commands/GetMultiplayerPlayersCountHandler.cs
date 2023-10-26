using MediatR;
using RockPaperScissors.Core.Domain.Interfaces;

namespace RockPaperScissors.Core.Business.Mediatr.Commands;

public class GetMultiplayerPlayersCountHandler(IPlayerCacheService playerCacheService) : IRequestHandler<GetMultiplayerPlayersCountCommand, int>
{
    public Task<int> Handle(GetMultiplayerPlayersCountCommand request, CancellationToken cancellationToken)
    {
        var playersCount = playerCacheService.GetPlayersCount();
        return Task.FromResult(playersCount);
    }
}
