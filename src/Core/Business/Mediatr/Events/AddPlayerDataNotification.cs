using MediatR;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Business.Mediatr.Events;

public class AddPlayerDataNotification : INotification
{
    public Player Player { get; set; }
}
