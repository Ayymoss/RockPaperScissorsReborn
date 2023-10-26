using MediatR;

namespace RockPaperScissors.Core.Business.Mediatr.Events;

public class RemoveMultiplayerPlayerNotification : INotification
{
    public string ConnectionId { get; set; }
}
