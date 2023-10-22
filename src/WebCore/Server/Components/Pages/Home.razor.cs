using MediatR;
using Microsoft.AspNetCore.Components;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.WebCore.Server.Components.Pages;

public partial class Home
{
    [Inject] private IMediator Mediator { get; set; }

    private Player? _player;

    private void OnPlayerUpdated(Player? updatedPlayer)
    {
        _player = updatedPlayer;
        StateHasChanged();
    }
}
