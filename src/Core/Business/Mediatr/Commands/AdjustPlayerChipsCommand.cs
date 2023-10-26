using MediatR;

namespace RockPaperScissors.Core.Business.Mediatr.Commands;

public class AdjustPlayerChipsCommand : IRequest<int?>
{
    public Guid PlayerGuid { get; set; }
    public int AddChips { get; set; }
}
