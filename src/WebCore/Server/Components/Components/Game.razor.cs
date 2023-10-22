using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using RockPaperScissors.Core.Business.Mediatr.Events;
using RockPaperScissors.Core.Domain.Enums;
using RockPaperScissors.Core.Domain.ValueObjects;
using RockPaperScissors.Core.Infrastructure.SignalR;

namespace RockPaperScissors.WebCore.Server.Components.Components;

public partial class Game
{
    [CascadingParameter] public Player? Player { get; set; }
    [Parameter] public EventCallback<Player?> OnPlayerChanged { get; set; }
    [Inject] public IMediator Mediator { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; }
 
    private ComputerGame? _computerGame;
    private bool _gameEnded;
    private int _finalStreak;

    public async Task InitialiseGame()
    {
        if (Player is null) return;
        _finalStreak = 0;
        _gameEnded = false;
        Player.CurrentGame = new CurrentGame
        {
            GameStarted = DateTimeOffset.UtcNow,
        };
        await OnPlayerChanged.InvokeAsync(Player);
        _computerGame = new ComputerGame();
        GenerateComputerMove();
    }

    private async Task SubmitMove(MouseEventArgs e, RockPaperScissorsMove move)
    {
        if (Player?.CurrentGame is null || _computerGame is null) return;

        var isWinner = IsUserWinner(move);
        if (isWinner)
        {
            Player.CurrentGame.Streak++;
            if (Player.BestStreak < Player.CurrentGame.Streak)
            {
                Player.BestStreak = Player.CurrentGame.Streak;
            }

            await OnPlayerChanged.InvokeAsync(Player);
            await JsRuntime.InvokeVoidAsync("playSound.correct");
        }
        else
        {
            _finalStreak = Player.CurrentGame.Streak;
            Player.CurrentGame.GameFinished = DateTimeOffset.UtcNow;
            await Mediator.Publish(new AddOrUpdateLeaderboardNotification {Player = Player});
            Player.CurrentGame = null;
            await OnPlayerChanged.InvokeAsync(Player);
            _gameEnded = true;
            _computerGame = null;
            await JsRuntime.InvokeVoidAsync("playSound.wrong");
        }

        GenerateComputerMove();
    }

    private bool IsUserWinner(RockPaperScissorsMove move)
    {
        if (Player?.CurrentGame is null || _computerGame is null) return false;
        return move switch
        {
            RockPaperScissorsMove.Rock => _computerGame.Move is RockPaperScissorsMove.Scissors,
            RockPaperScissorsMove.Paper => _computerGame.Move is RockPaperScissorsMove.Rock,
            RockPaperScissorsMove.Scissors => _computerGame.Move is RockPaperScissorsMove.Paper,
            _ => false
        };
    }

    private void GenerateComputerMove()
    {
        if (_computerGame is null) return;
        _computerGame.Move = (RockPaperScissorsMove)Random.Shared.Next(0, 3);
    }
}
