using MediatR;
using RockPaperScissors.Core.Domain.Enums;
using RockPaperScissors.Core.Domain.Interfaces;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Business.Mediatr.Commands;

public class AddOrUpdateMultiplayerGameHandler(IMultiplayerCacheService multiplayerCacheService, IPlayerCacheService playerCacheService,
        INotificationService notificationService)
    : IRequestHandler<AddOrUpdateMultiplayerGameCommand, bool>
{
    public async Task<bool> Handle(AddOrUpdateMultiplayerGameCommand request, CancellationToken cancellationToken)
    {
        if (!multiplayerCacheService.TryGetGame(request.MatchId, out var gameContext))
        {
            gameContext = new MultiplayerGameStateContext
            {
                PlayerOne = request.Player,
                Private = request.Private,
            };

            multiplayerCacheService.AddOrUpdateGame(request.MatchId, gameContext);
            return false;
        }

        if (gameContext is null) throw new InvalidOperationException("Game context is null"); // Should never happen
        if (gameContext.PlayerTwo is not null) throw new InvalidOperationException("Game is full");

        gameContext.PlayerTwo = request.Player;
        multiplayerCacheService.AddOrUpdateGame(request.MatchId, gameContext);

        playerCacheService.TryGetConnectionId(gameContext.PlayerOne, out var connectionId);
        await notificationService.SendToClient(connectionId, SignalRMethods.NotifyGameReady, null, cancellationToken);

        return true;
    }
}
