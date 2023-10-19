using Microsoft.AspNetCore.SignalR.Client;
using RockPaperScissors.Enums;
using RockPaperScissors.Models;

namespace RockPaperScissors.Services.Client;

public class RpsClientHub : IAsyncDisposable
{
    public event Action<SessionIdInfo>? SessionIdChanged;
    private HubConnection? _hubConnection;

    public async Task InitializeAsync()
    {
        CreateHubConnection();
        await StartConnection();
        //SubscribeToHubEvents();
        await FetchSessionId();
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
        _hubConnection?.On<SessionIdInfo>(SignalRMethods.OnConnectionId, id => SessionIdChanged?.Invoke(id));
    }

    private async Task FetchSessionId()
    {
        if (_hubConnection is null) return;
        var onlineCount = await _hubConnection.InvokeAsync<SessionIdInfo>(SignalRMethods.GetConnectionId);
        SessionIdChanged?.Invoke(onlineCount);
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
