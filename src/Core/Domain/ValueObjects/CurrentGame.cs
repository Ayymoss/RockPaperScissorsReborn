namespace RockPaperScissors.Core.Domain.ValueObjects;

public class CurrentGame
{
    public int Streak { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTimeOffset Submitted { get; set; }
}
