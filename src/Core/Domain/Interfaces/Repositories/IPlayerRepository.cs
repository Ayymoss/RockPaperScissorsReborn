using RockPaperScissors.Core.Domain.Entities;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Domain.Interfaces.Repositories;

public interface IPlayerRepository
{
    Task<EFPlayer?> GetPlayerDataAsync(Guid guid, CancellationToken cancellationToken);
    Task<int> CreatePlayerAsync(EFPlayer player, CancellationToken cancellationToken);
}
