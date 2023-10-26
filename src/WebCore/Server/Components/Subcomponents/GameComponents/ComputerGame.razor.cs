using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RockPaperScissors.Core.Business.Mediatr.Commands;
using RockPaperScissors.Core.Business.Mediatr.Events;
using RockPaperScissors.Core.Domain.Enums;
using RockPaperScissors.Core.Domain.Interfaces;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.WebCore.Server.Components.Subcomponents.GameComponents;

public partial class ComputerGame
{
    [Parameter] public GameMode GameMode { get; set; }
    [Parameter] public EventCallback<GameState> OnGameEnd { get; set; }
    [Inject] public IGameLogicService GameLogicService { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; }

    private GameState? _gameState;
    private bool _gameEnded;

    public async Task ReturnToMain()
    {
        await OnGameEnd.InvokeAsync(_gameState);
        _gameState = null;
        _gameEnded = false;
    }

    private async Task SubmitMove(GameMove move)
    {
        if (_gameState is null) GameSetup();

        var computerMove = GameLogicService.GenerateComputerMove();
        var isWinner = GameLogicService.IsUserWinner(move, computerMove);
        if (isWinner)
        {
            _gameState!.Streak++;
            await JsRuntime.InvokeVoidAsync("playSound.correct");
        }
        else
        {
            _gameState!.Ended = DateTimeOffset.UtcNow;
            _gameState!.Payout = GameLogicService.CalculatePayout(_gameState!.Streak);
            _gameEnded = true;
            await JsRuntime.InvokeVoidAsync("playSound.wrong");
        }
    }

    private void GameSetup()
    {
        _gameEnded = false;
        _gameState ??= new GameState();
        _gameState.Started = DateTimeOffset.UtcNow;
    }
}
