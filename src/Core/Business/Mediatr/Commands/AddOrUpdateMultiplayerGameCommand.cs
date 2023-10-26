using MediatR;

namespace RockPaperScissors.Core.Business.Mediatr.Commands;

public class AddOrUpdateMultiplayerGameCommand : IRequest<bool>
{
    public string MatchId { get; set; }
    public Guid Player { get; set; }
    public bool Private { get; set; }
}
