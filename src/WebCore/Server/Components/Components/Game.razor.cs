using Microsoft.AspNetCore.Components;
using RockPaperScissors.Core.Domain.ValueObjects;
using RockPaperScissors.Core.Infrastructure.SignalR;

namespace RockPaperScissors.WebCore.Server.Components.Components;

public partial class Game
{
    [Inject] private Player Player { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Player.OnPlayerDataLoad += () => InvokeAsync(StateHasChanged);
        await base.OnInitializedAsync();
    }
}
