using MediatR;
using RockPaperScissors.Core.Domain.Interfaces;

namespace RockPaperScissors.Core.Business.Mediatr.Events;

public class RemoveMultiplayerPlayerHandler(IPlayerCacheService playerCacheService)
    : INotificationHandler<RemoveMultiplayerPlayerNotification>
{
    public Task Handle(RemoveMultiplayerPlayerNotification notification, CancellationToken cancellationToken)
    {
        playerCacheService.RemovePlayer(notification.ConnectionId);
        return Task.CompletedTask;
    }
}
