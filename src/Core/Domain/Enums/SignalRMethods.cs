namespace RockPaperScissors.Core.Domain.Enums;

public enum SignalRMethods
{
    #region Client Methods

    Pong,
    OnMultiplayerPlayersCountUpdate,
    OnLeaderboardUpdate,

    #endregion

    #region Server Methods

    Ping,
    UpdatePlayer,
    GetOnlinePlayers,
    NotifyGameReady

    #endregion
}
