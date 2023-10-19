namespace RockPaperScissors.Models;

public class LeaderboardContext
{
    public string UserName { get; set; }
    public int Streak { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTimeOffset Submitted { get; set; }
}
