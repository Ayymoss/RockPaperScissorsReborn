using RockPaperScissors.Core.Domain.Enums;

namespace RockPaperScissors.Core.Domain.ValueObjects;

/// <summary>
/// This is a server-side property for game state tracking.
/// </summary>
public class MultiplayerMoveState
{
    public GameMove? PlayerOneMove { get; set; }
    public GameMove? PlayerTwoMove { get; set; }
}