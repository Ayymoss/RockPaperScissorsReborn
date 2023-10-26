using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RockPaperScissors.Core.Business.Mediatr.Commands;
using RockPaperScissors.Core.Business.Mediatr.Events;
using RockPaperScissors.Core.Domain.Enums;
using RockPaperScissors.Core.Domain.ValueObjects;
using RockPaperScissors.WebCore.Server.Components.Subcomponents.GameComponents;

namespace RockPaperScissors.WebCore.Server.Components.Subcomponents;

public partial class GameWrapper
{
    [CascadingParameter] public Player? Player { get; set; }
    [Parameter] public EventCallback<Player?> OnPlayerChanged { get; set; }
    [Parameter] public EventCallback<MultiplayerGameState?> OnStartMultiplayer { get; set; }
    [Inject] public IMediator Mediator { get; set; }

    private ChooseMode _chooseModeComponent;
    private ComputerGame _computerGameComponent;
    private MultiplayerGame _multiplayerGameComponent;

    private GameMode _currentGameMode;


    private void HandleModeChosen(GameMode gameMode)
    {
        _currentGameMode = gameMode;
    }

    private async Task HandleGameEnd(GameState gameState)
    {
        if (Player is null) return;
        _currentGameMode = GameMode.Unknown;

        if (gameState.Payout is not 0)
            Player.Chips = await Mediator.Send(new AdjustPlayerChipsCommand
            {
                PlayerGuid = Player.Guid,
                AddChips = gameState.Payout
            }) ?? 0;

        if (gameState.Streak is not 0)
            await Mediator.Publish(new AddOrUpdateLeaderboardNotification
            {
                PlayerGuid = Player.Guid,
                GameState = gameState,
            });

        await OnPlayerChanged.InvokeAsync(Player);
    }
}
