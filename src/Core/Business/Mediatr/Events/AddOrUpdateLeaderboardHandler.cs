using MediatR;
using RockPaperScissors.Core.Domain.Entities;
using RockPaperScissors.Core.Domain.Enums;
using RockPaperScissors.Core.Domain.Interfaces;
using RockPaperScissors.Core.Domain.Interfaces.Repositories;

namespace RockPaperScissors.Core.Business.Mediatr.Events;

public class AddOrUpdateLeaderboardHandler(ILeaderboardRepository leaderboardRepository, IPlayerRepository playerRepository,
        INotificationService notificationService)
    : INotificationHandler<AddOrUpdateLeaderboardNotification>
{
    public async Task Handle(AddOrUpdateLeaderboardNotification notification, CancellationToken cancellationToken)
    {
        var player = await playerRepository.GetPlayerDataAsync(notification.PlayerGuid, cancellationToken);
        if (player is null) return;

        var currentLeaderboard = await leaderboardRepository.GetLeaderboardAsync(notification.PlayerGuid, cancellationToken);
        if (currentLeaderboard is null)
        {
            currentLeaderboard = new EFLeaderboard
            {
                Streak = notification.GameState.Streak,
                Duration = notification.GameState.Ended - notification.GameState.Started,
                Submitted = DateTimeOffset.UtcNow,
                PlayerId = player.Id,
            };
        }
        else
        {
            if (currentLeaderboard.Streak > notification.GameState.Streak) return;
            currentLeaderboard.Streak = notification.GameState.Streak;
            currentLeaderboard.Duration = notification.GameState.Ended - notification.GameState.Started;
            currentLeaderboard.Submitted = DateTimeOffset.UtcNow;
        }

        await leaderboardRepository.AddOrUpdateLeaderboardAsync(currentLeaderboard, cancellationToken);
        await notificationService.SendToAllClients(SignalRMethods.OnLeaderboardUpdate, cancellationToken);
    }
}
