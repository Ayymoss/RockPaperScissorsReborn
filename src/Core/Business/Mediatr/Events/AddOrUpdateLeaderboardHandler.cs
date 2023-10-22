using MediatR;
using RockPaperScissors.Core.Domain.Entities;
using RockPaperScissors.Core.Domain.Interfaces.Repositories;

namespace RockPaperScissors.Core.Business.Mediatr.Events;

public class AddOrUpdateLeaderboardHandler(ILeaderboardRepository leaderboardRepository, IPlayerRepository playerRepository) 
    : INotificationHandler<AddOrUpdateLeaderboardNotification>
{
    public async Task Handle(AddOrUpdateLeaderboardNotification notification, CancellationToken cancellationToken)
    {
        if (notification.Player.CurrentGame is null) return;

        var player = await playerRepository.GetPlayerDataAsync(notification.Player.Guid, cancellationToken);
        if (player is null) return;

        var currentLeaderboard = await leaderboardRepository.GetLeaderboardAsync(notification.Player.Guid, cancellationToken);
        if (currentLeaderboard is null)
        {
            currentLeaderboard = new EFLeaderboard
            {
                Streak = notification.Player.CurrentGame.Streak,
                Duration = notification.Player.CurrentGame.GameFinished - notification.Player.CurrentGame.GameStarted,
                Submitted = DateTimeOffset.UtcNow,
                PlayerId = player.Id,
            };
        }
        else
        {
            if (currentLeaderboard.Streak < notification.Player.CurrentGame.Streak)
            {
                currentLeaderboard.Streak = notification.Player.CurrentGame.Streak;
                currentLeaderboard.Duration = notification.Player.CurrentGame.GameFinished - notification.Player.CurrentGame.GameStarted;
                currentLeaderboard.Submitted = DateTimeOffset.UtcNow;
            }
            else return;
        }

        await leaderboardRepository.AddOrUpdateLeaderboardAsync(currentLeaderboard, cancellationToken);
    }
}
