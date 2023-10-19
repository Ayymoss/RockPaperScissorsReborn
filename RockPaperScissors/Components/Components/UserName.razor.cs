using Microsoft.AspNetCore.Components;
using RockPaperScissors.Models;
using RockPaperScissors.Services;
using RockPaperScissors.Services.Client;

namespace RockPaperScissors.Components.Components;

public partial class UserName : IAsyncDisposable
{
    [Inject] ClientPageService ClientPageService { get; set; }
    [Inject] RpsClientHub RpsClientHub { get; set; }

    private string _nameError = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        ClientPageService.OnNameProvided += async () => await InvokeAsync(StateHasChanged);
        RpsClientHub.SessionIdChanged += SetSessionId;
        await RpsClientHub.InitializeAsync();
        await base.OnInitializedAsync();
    }

    private void UpdateUserName(string userName)
    {
        _nameError = string.Empty;
        if (!string.IsNullOrWhiteSpace(userName) && userName.Length is > 2 and < 17)
        {
            ClientPageService.NameHasChanged(userName);
            return;
        }

        _nameError = "Name must be between 3 and 16 characters long.";
    }

    private void SetSessionId(SessionIdInfo identity)
    {
        InvokeAsync(() =>
        {
            ClientPageService.SetSessionId(identity.Identity);
            StateHasChanged();
        });
    }

    public async ValueTask DisposeAsync()
    {
        RpsClientHub.SessionIdChanged -= SetSessionId;
        await RpsClientHub.DisposeAsync();
    }
}
