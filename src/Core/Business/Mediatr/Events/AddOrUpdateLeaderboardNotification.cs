using MediatR;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Business.Mediatr.Events;

public class AddOrUpdateLeaderboardNotification : INotification
{
    public Guid PlayerGuid { get; set; }
    public GameState GameState { get; set; }
}
