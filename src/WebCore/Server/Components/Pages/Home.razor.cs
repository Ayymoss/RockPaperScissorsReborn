using MediatR;
using Microsoft.AspNetCore.Components;
using RockPaperScissors.Core.Domain.ValueObjects;
using RockPaperScissors.Core.Infrastructure.SignalR;
using Leaderboard = RockPaperScissors.WebCore.Server.Components.Subcomponents.Leaderboard;

namespace RockPaperScissors.WebCore.Server.Components.Pages;

public partial class Home : IAsyncDisposable
{
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private RpsClientHub? HubConnection { get; set; }

    private Player? _player;
    private Leaderboard _leaderboardComponent;

    private long _pingTimestamp;
    private int _latencyInMilliseconds;
    private int _onlinePlayers;

    protected override async Task OnInitializedAsync()
    {
        if (HubConnection is null) return;
        HubConnection.OnPingPongReceived += OnPingPongReceived;
        HubConnection.OnPlayerCountChanged += OnPlayerCountChangedReceived;
        HubConnection.OnLeaderboardUpdate += OnLeaderboardUpdateReceived;
        _ = PeriodicPingAsync();
        await base.OnInitializedAsync();
    }


    private async Task OnMultiplayerStarted(MultiplayerGameState? multiplayer)
    {
        StateHasChanged();
    }
    
    private async Task OnPlayerUpdated(Player? updatedPlayer)
    {
        _player = updatedPlayer;
        await InformServerOfPlayerUpdateAsync();
        StateHasChanged();
    }

    private async Task InformServerOfPlayerUpdateAsync()
    {
        if (_player is null || HubConnection is null) return;
        await HubConnection.UpdatePlayerAsync(_player);
    }

    private async Task PeriodicPingAsync()
    {
        while (true)
        {
            await Task.Delay(5000);
            await MeasureLatency();
        }
    }

    private async Task MeasureLatency()
    {
        if (string.IsNullOrWhiteSpace(_player?.UserName) || HubConnection is null) return;
        _pingTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await HubConnection.PingAsync();
    }

    private void OnLeaderboardUpdateReceived()
    {
        try
        {
            _ = HandleLeaderboardUpdateReceivedAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private async Task HandleLeaderboardUpdateReceivedAsync()
    {
        await _leaderboardComponent.ReloadData();
    }

    private void OnPlayerCountChangedReceived(int count)
    {
        try
        {
            _ = HandlePlayerCountChangedReceivedAsync(count).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private async Task HandlePlayerCountChangedReceivedAsync(int count)
    {
        _onlinePlayers = count;
        await InvokeAsync(StateHasChanged);
    }

    private void OnPingPongReceived()
    {
        try
        {
            _ = HandlePingPongReceivedAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private async Task HandlePingPongReceivedAsync()
    {
        var pongTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        _latencyInMilliseconds = (int)(pongTimestamp - _pingTimestamp);
        await InvokeAsync(StateHasChanged);
    }

    public async ValueTask DisposeAsync()
    {
        if (HubConnection is null) return;
        HubConnection.OnPingPongReceived -= OnPingPongReceived;
        await HubConnection.DisposeAsync();
    }
}
