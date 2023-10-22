using System.ComponentModel.DataAnnotations;

namespace RockPaperScissors.Core.Domain.Entities;

public class EFPlayer
{
    [Key] public int Id { get; set; }
    public Guid Guid { get; set; }
    public string UserName { get; set; }
    public int Chips { get; set; }
    public EFLeaderboard? Leaderboard { get; set; }
}
