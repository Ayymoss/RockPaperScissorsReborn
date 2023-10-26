using System.Collections.Concurrent;
using RockPaperScissors.Core.Domain.Interfaces;

namespace RockPaperScissors.Core.Infrastructure.Services;

public class PlayerCacheService : IPlayerCacheService
{
    private readonly ConcurrentDictionary<string, Guid> _playerCache = new();

    public void AddOrUpdatePlayer(string connectionId, Guid playerGuid)
    {
        _playerCache.AddOrUpdate(connectionId, playerGuid, (_, _) => playerGuid);
    }

    public bool TryGetPlayerGuid(string connectionId, out Guid playerGuid)
    {
        return _playerCache.TryGetValue(connectionId, out playerGuid);
    }
    
    public bool TryGetConnectionId(Guid playerGuid, out string connectionId)
    {
        connectionId = _playerCache.FirstOrDefault(x => x.Value == playerGuid).Key;
        return connectionId != null;
    }

    public void RemovePlayer(string connectionId)
    {
        _playerCache.TryRemove(connectionId, out _);
    }

    public int GetPlayersCount()
    {
        return _playerCache.Count;
    }
}
