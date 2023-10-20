using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Domain.Interfaces.Repositories;

public interface ILeaderboardRepository
{
    Task<Player?> GetPlayerDataAsync(Guid guid, CancellationToken cancellationToken);
    Task<int> CreatePlayerAsync(Player player, CancellationToken cancellationToken);
    Task UpdatePlayerAsync(Player player, CancellationToken cancellationToken);
}
