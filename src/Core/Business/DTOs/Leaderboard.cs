namespace RockPaperScissors.Core.Business.DTOs;

public class Leaderboard
{
    public int Rank { get; set; }
    public string UserName { get; set; }
    public int Streak { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTimeOffset Submitted { get; set; }
}
