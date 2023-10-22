using RockPaperScissors.Core.Domain.Entities;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Domain.Interfaces.Repositories;

public interface ILeaderboardRepository
{
    Task<int> GetPlayerStreakAsync(Guid guid, CancellationToken cancellationToken);
    Task<int> GetPlayersWithBetterStreakAsync(int playerStreak, CancellationToken cancellationToken);
    Task<IEnumerable<EFLeaderboard>> GetPlayersWithSameStreakAsync(int playerStreak, CancellationToken cancellationToken);
    Task<EFLeaderboard?> GetLeaderboardAsync(Guid guid, CancellationToken cancellationToken);
    Task<int> AddOrUpdateLeaderboardAsync(EFLeaderboard leaderboard, CancellationToken cancellationToken);
}
