using Microsoft.AspNetCore.SignalR.Client;
using RockPaperScissors.Core.Domain.Enums;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Infrastructure.SignalR;

public class RpsClientHub : IAsyncDisposable
{
    public event Action? OnPingPongReceived;
    public event Action<int>? OnPlayerCountChanged;
    public event Action? OnLeaderboardUpdate;
    private HubConnection? _hubConnection;

    public async Task InitializeAsync()
    {
        CreateHubConnection();
        await StartConnection();
        SubscribeToHubEvents();
        await InitialiseDefaults();
    }

    private void CreateHubConnection()
    {
        _hubConnection = new HubConnectionBuilder()
#if DEBUG
            .WithUrl("http://localhost:8123/SignalR/RpsServerHub")
#else
            .WithUrl("https://banhub.gg/SignalR/RpsServerHub")
#endif
            .WithAutomaticReconnect()
            .Build();
    }

    private async Task StartConnection()
    {
        if (_hubConnection is not null) await _hubConnection.StartAsync();
    }

    private void SubscribeToHubEvents()
    {
        _hubConnection?.On(SignalRMethods.Pong.ToString(), () => OnPingPongReceived?.Invoke());
        _hubConnection?.On<int>(SignalRMethods.OnMultiplayerPlayersCountUpdate.ToString(), count => OnPlayerCountChanged?.Invoke(count));
        _hubConnection?.On(SignalRMethods.OnLeaderboardUpdate.ToString(), () => OnLeaderboardUpdate?.Invoke());
    }

    private async Task InitialiseDefaults()
    {
        if (_hubConnection is null) return;
        var onlineCount = await _hubConnection.InvokeAsync<int>(SignalRMethods.GetOnlinePlayers.ToString());
        OnPlayerCountChanged?.Invoke(onlineCount);
    }

    public async Task PingAsync()
    {
        if (_hubConnection is null) return;
        await _hubConnection.InvokeAsync(SignalRMethods.Ping.ToString());
    }

    public async Task UpdatePlayerAsync(Player player)
    {
        if (_hubConnection is null) return;
        await _hubConnection.InvokeAsync(SignalRMethods.UpdatePlayer.ToString(), player);
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is {State: HubConnectionState.Connected})
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
        }
    }
}
