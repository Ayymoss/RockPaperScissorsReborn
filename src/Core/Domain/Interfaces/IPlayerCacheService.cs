namespace RockPaperScissors.Core.Domain.Interfaces;

public interface IPlayerCacheService
{
    void AddOrUpdatePlayer(string connectionId, Guid playerGuid);
    bool TryGetConnectionId(Guid playerGuid, out string connectionId);
    bool TryGetPlayerGuid(string connectionId, out Guid playerGuid);
    void RemovePlayer(string connectionId);
    int GetPlayersCount();
}
