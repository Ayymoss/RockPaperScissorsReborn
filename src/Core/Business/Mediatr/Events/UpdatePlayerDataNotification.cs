using MediatR;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Business.Mediatr.Events;

public class UpdatePlayerDataNotification : INotification
{
    public Player Player { get; set; }
}
