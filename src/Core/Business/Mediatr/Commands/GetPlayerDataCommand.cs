using MediatR;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Business.Mediatr.Commands;

public class GetPlayerDataCommand : IRequest<Player?>
{
    public Guid PlayerGuid { get; set; }
}
