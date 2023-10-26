using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Domain.Interfaces;

public interface IMultiplayerCacheService
{
    void AddOrUpdateGame(string matchId, MultiplayerGameStateContext gameContext);
    bool TryGetGame(string matchId, out MultiplayerGameStateContext? gameStateContext);
    bool RemoveGame(string matchId);
    int GetGameCount();
}
