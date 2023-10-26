using MediatR;

namespace RockPaperScissors.Core.Business.Mediatr.Events;

public class UpdateMultiplayerPlayerNotification : INotification
{
    public string ConnectionId { get; set; }
    public Guid PlayerGuid { get; set; }
}
