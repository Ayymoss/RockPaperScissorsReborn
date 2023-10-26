using System.Collections.Concurrent;
using RockPaperScissors.Core.Domain.Interfaces;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Infrastructure.Services;

public class MultiplayerCacheService : IMultiplayerCacheService
{
    private readonly ConcurrentDictionary<string, MultiplayerGameStateContext> _multiplayerGames = new();

    public void AddOrUpdateGame(string matchId, MultiplayerGameStateContext gameContext)
    {
        _multiplayerGames.AddOrUpdate(matchId, gameContext, (_, _) => gameContext);
    }

    public bool TryGetGame(string matchId, out MultiplayerGameStateContext? gameStateContext)
    {
        return _multiplayerGames.TryGetValue(matchId, out gameStateContext);
    }

    public bool RemoveGame(string matchId)
    {
        return _multiplayerGames.TryRemove(matchId, out _);
    }

    public int GetGameCount()
    {
        return _multiplayerGames.Count;
    }
}
