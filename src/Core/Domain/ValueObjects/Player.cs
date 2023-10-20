namespace RockPaperScissors.Core.Domain.ValueObjects;

public class Player
{
    public event Action? OnPlayerDataLoad;
    public bool IsPlayerDataLoaded { get; set; }
    public Guid Guid { get; set; }
    public string UserName { get; set; }
    public int BestStreak { get; set; }
    public CurrentGame? CurrentGame { get; set; }

    public void NotifyPlayerDataLoad()
    {
        IsPlayerDataLoaded = true;
        OnPlayerDataLoad?.Invoke();
    }
}
