using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RockPaperScissors.Core.Domain.Entities;

public class EFLeaderboard
{
    [Key] public int Id { get; set; }
    public int Streak { get; set; }
    public TimeSpan? Duration { get; set; }
    public DateTimeOffset? Submitted { get; set; }
    public int PlayerId { get; set; }

    [ForeignKey(nameof(PlayerId))] public EFPlayer Player { get; set; }
}
