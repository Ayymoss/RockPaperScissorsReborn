using System.ComponentModel.DataAnnotations;

namespace RockPaperScissors.Core.Domain.Entities;

public class EFLeaderboard
{
    [Key] public int Id { get; set; }
    public Guid Guid { get; set; }
    public string UserName { get; set; }
    public int Streak { get; set; }
    public TimeSpan? Duration { get; set; }
    public DateTimeOffset? Submitted { get; set; }
}
