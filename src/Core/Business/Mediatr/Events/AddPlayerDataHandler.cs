using MediatR;
using RockPaperScissors.Core.Domain.Entities;
using RockPaperScissors.Core.Domain.Interfaces.Repositories;

namespace RockPaperScissors.Core.Business.Mediatr.Events;

public class AddPlayerDataHandler(IPlayerRepository playerRepository) : INotificationHandler<AddPlayerDataNotification>
{
    public async Task Handle(AddPlayerDataNotification notification, CancellationToken cancellationToken)
    {
        var player = new EFPlayer
        {
            Guid = notification.Player.Guid,
            UserName = notification.Player.UserName,
            Chips = 0
        };
        await playerRepository.CreatePlayerAsync(player, cancellationToken);
    }
}
