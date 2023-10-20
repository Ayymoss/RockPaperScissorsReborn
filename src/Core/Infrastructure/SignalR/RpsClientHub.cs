using Microsoft.AspNetCore.SignalR.Client;
using RockPaperScissors.Core.Domain.Enums;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Infrastructure.SignalR;

public class RpsClientHub : IAsyncDisposable
{
    public event Action<SessionIdInfoSignalR>? SessionIdChanged;
    private HubConnection? _hubConnection;

    public async Task InitializeAsync()
    {
        CreateHubConnection();
        await StartConnection();
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
    
    private async Task FetchSessionId()
    {
        if (_hubConnection is null) return;
        var onlineCount = await _hubConnection.InvokeAsync<SessionIdInfoSignalR>(SignalRMethods.GetConnectionId);
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
