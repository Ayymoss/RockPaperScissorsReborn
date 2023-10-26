namespace RockPaperScissors.Core.Domain.ValueObjects;

/// <summary>
/// This is a server-side property for game state tracking.
/// </summary>
public class MultiplayerGameStateContext
{
    public Guid PlayerOne { get; set; }
    public Guid? PlayerTwo { get; set; }
    public bool Private { get; set; }
    public MultiplayerMoveState? Moves { get; set; }
}
