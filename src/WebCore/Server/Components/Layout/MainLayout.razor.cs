using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using RockPaperScissors.Core.Infrastructure.SignalR;

namespace RockPaperScissors.WebCore.Server.Components.Layout;

public partial class MainLayout : IAsyncDisposable
{
    [Inject] private RpsClientHub? HubConnection { get; set; }

    private RadzenBody _body;

    protected override async Task OnInitializedAsync()
    {
        if (HubConnection is null) return;
        await HubConnection.InitializeAsync();
        await base.OnInitializedAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (HubConnection != null) await HubConnection.DisposeAsync();
    }
}
