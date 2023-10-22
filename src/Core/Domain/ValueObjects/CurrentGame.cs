namespace RockPaperScissors.Core.Domain.ValueObjects;

public class CurrentGame
{
    public int Streak { get; set; }
    public DateTimeOffset GameStarted { get; set; }
    public DateTimeOffset GameFinished { get; set; }
}
