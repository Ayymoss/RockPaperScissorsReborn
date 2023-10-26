using MediatR;
using RockPaperScissors.Core.Business.Mediatr.Commands;
using RockPaperScissors.Core.Domain.Enums;
using RockPaperScissors.Core.Domain.Interfaces;

namespace RockPaperScissors.Core.Business.Mediatr.Events;

public class UpdateMultiplayerPlayerHandler(IPlayerCacheService playerCacheService, ISender sender,
        INotificationService notificationService)
    : INotificationHandler<UpdateMultiplayerPlayerNotification>
{
    public async Task Handle(UpdateMultiplayerPlayerNotification notification, CancellationToken cancellationToken)
    {
        playerCacheService.AddOrUpdatePlayer(notification.ConnectionId, notification.PlayerGuid);
        var count = await sender.Send(new GetMultiplayerPlayersCountCommand(), cancellationToken);
        await notificationService.SendToAllClients(SignalRMethods.OnMultiplayerPlayersCountUpdate, count, cancellationToken);
    }
}
