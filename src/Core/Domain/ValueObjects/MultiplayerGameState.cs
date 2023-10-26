namespace RockPaperScissors.Core.Domain.ValueObjects;

/// <summary>
/// This is for the individual client, is not server-synced.
/// </summary>
public class MultiplayerGameState
{
    public string? MatchGuid { get; set; }
    public bool Private { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
}
