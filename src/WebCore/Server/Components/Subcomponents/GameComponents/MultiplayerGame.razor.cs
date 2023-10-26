using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RockPaperScissors.Core.Domain.Enums;
using RockPaperScissors.Core.Domain.Interfaces;
using RockPaperScissors.Core.Domain.ValueObjects;
using RockPaperScissors.Core.Infrastructure.SignalR;

namespace RockPaperScissors.WebCore.Server.Components.Subcomponents.GameComponents;

public partial class MultiplayerGame
{
    [Parameter] public Guid? PlayerGuid { get; set; }
    [Parameter] public GameMode GameMode { get; set; }
    [Parameter] public EventCallback<GameState> OnGameEnd { get; set; }
    [Inject] public IGameLogicService GameLogicService { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; }
    [Inject] private RpsClientHub? HubConnection { get; set; }

    private GameState? _gameState;
    private bool _gameEnded;
    private bool _waitingForOpponent;

    public async Task ReturnToMain()
    {
        await OnGameEnd.InvokeAsync(_gameState);
        _gameState = null;
        _gameEnded = false;
    }

    private async Task SubmitMove(GameMove move)
    {
        if (_gameState is null) GameSetup();

        {
            await JsRuntime.InvokeVoidAsync("playSound.correct");
        }
        {
            await JsRuntime.InvokeVoidAsync("playSound.wrong");
        }
    }
    
    

    private void EndGame()
    {
        _gameState!.Ended = DateTimeOffset.UtcNow;
        _gameState!.Payout = GameLogicService.CalculatePayout(_gameState!.Streak);
        _gameEnded = true;
    }

    private void GameSetup()
    {
        _gameEnded = false;
        _gameState ??= new GameState();
        _gameState.Started = DateTimeOffset.UtcNow;
    }
}
