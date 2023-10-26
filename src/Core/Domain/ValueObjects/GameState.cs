namespace RockPaperScissors.Core.Domain.ValueObjects;

public class GameState
{
    public DateTimeOffset Started { get; set; }
    public DateTimeOffset Ended { get; set; }
    public int Streak { get; set; }
    public int Payout { get; set; }
}
