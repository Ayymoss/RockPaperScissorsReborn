using MediatR;
using RockPaperScissors.Core.Domain.Interfaces.Repositories;

namespace RockPaperScissors.Core.Business.Mediatr.Events;

public class AddPlayerDataHandler(ILeaderboardRepository leaderboardRepository) : INotificationHandler<AddPlayerDataNotification>
{
    public async Task Handle(AddPlayerDataNotification notification, CancellationToken cancellationToken)
    {
        await leaderboardRepository.CreatePlayerAsync(notification.Player, cancellationToken);
    }
}
