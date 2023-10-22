namespace RockPaperScissors.Core.Domain.ValueObjects;

public class Player
{
    public bool IsPlayerDataLoaded { get; set; }
    public Guid Guid { get; set; }
    public string UserName { get; set; }
    public int Chips { get; set; }
    public int? BestStreak { get; set; }
    public int? Placement { get; set; }
    public CurrentGame? CurrentGame { get; set; }
}
