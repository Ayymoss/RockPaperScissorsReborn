using MediatR;
using RockPaperScissors.Core.Domain.Interfaces.Repositories;

namespace RockPaperScissors.Core.Business.Mediatr.Events;

public class UpdatePlayerDataHandler(ILeaderboardRepository leaderboardRepository) : INotificationHandler<UpdatePlayerDataNotification>
{
    public async Task Handle(UpdatePlayerDataNotification notification, CancellationToken cancellationToken)
    {
        await leaderboardRepository.UpdatePlayerAsync(notification.Player, cancellationToken);
    }
}
