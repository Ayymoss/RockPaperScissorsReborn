using Microsoft.AspNetCore.Components;
using RockPaperScissors.Core.Domain.Enums;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.WebCore.Server.Components.Subcomponents.GameComponents;

public partial class ChooseMode
{
    [Parameter] public GameMode GameMode { get; set; }
    [Parameter] public EventCallback<GameMode> OnModeSelected { get; set; }
    [Parameter] public EventCallback<MultiplayerGameState>

    private async Task ChooseComputer() => await OnModeSelected.InvokeAsync(GameMode.Computer);

    private async Task ChooseMultiplayerPrivate()
    {
        await OnModeSelected.InvokeAsync(mode);
    }
    private async Task ChooseMultiplayerMatchmaking()
    {
        await OnModeSelected.InvokeAsync(mode);
    } 
    
}
