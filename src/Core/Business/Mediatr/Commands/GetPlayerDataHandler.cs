using MediatR;
using RockPaperScissors.Core.Domain.Entities;
using RockPaperScissors.Core.Domain.Interfaces.Repositories;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Business.Mediatr.Commands;

public class GetPlayerDataHandler
    (IPlayerRepository playerRepository, ILeaderboardRepository leaderboardRepository) : IRequestHandler<GetPlayerDataCommand, Player?>
{
    public async Task<Player?> Handle(GetPlayerDataCommand request, CancellationToken cancellationToken)
    {
        var efPlayer = await playerRepository.GetPlayerDataAsync(request.PlayerGuid, cancellationToken);
        if (efPlayer is null) return null;

        var rank = await CalculatePlacementAsync(request.PlayerGuid, cancellationToken);

        var player = new Player
        {
            Guid = efPlayer.Guid,
            UserName = efPlayer.UserName,
            BestStreak = efPlayer.Leaderboard?.Streak,
            Placement = rank,
            Chips = efPlayer.Chips
        };
        return player;
    }

    private async Task<int?> CalculatePlacementAsync(Guid guid, CancellationToken cancellationToken)
    {
        var playerStreak = await leaderboardRepository.GetPlayerStreakAsync(guid, cancellationToken);
        if (playerStreak is 0) return null;

        var playersWithBetterStreak = await leaderboardRepository.GetPlayersWithBetterStreakAsync(playerStreak, cancellationToken);
        var sameStreakPlayers = await leaderboardRepository.GetPlayersWithSameStreakAsync(playerStreak, cancellationToken);

        var rankWithinStreak = 1;
        foreach (var leaderboard in sameStreakPlayers)
        {
            if (leaderboard.Player.Guid == guid)
            {
                return playersWithBetterStreak + rankWithinStreak;
            }

            rankWithinStreak++;
        }

        return null;
    }
}
