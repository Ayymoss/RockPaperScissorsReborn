using MediatR;
using RockPaperScissors.Core.Domain.Interfaces.Repositories;

namespace RockPaperScissors.Core.Business.Mediatr.Commands;

public class AdjustPlayerChipsHandler(IPlayerRepository playerRepository) : IRequestHandler<AdjustPlayerChipsCommand, int?>
{
    public async Task<int?> Handle(AdjustPlayerChipsCommand request, CancellationToken cancellationToken)
    {
        var newPlayerChips = await playerRepository.AdjustPlayerChipsAsync(request.PlayerGuid, request.AddChips, cancellationToken);
        return newPlayerChips;
    }
}
