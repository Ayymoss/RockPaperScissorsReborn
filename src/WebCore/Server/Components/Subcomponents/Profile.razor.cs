using Blazored.LocalStorage;
using MediatR;
using Microsoft.AspNetCore.Components;
using RockPaperScissors.Core.Business.Mediatr.Commands;
using RockPaperScissors.Core.Business.Mediatr.Events;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.WebCore.Server.Components.Subcomponents;

public partial class Profile
{
    [CascadingParameter] public Player? Player { get; set; }
    [Parameter] public EventCallback<Player?> OnPlayerChanged { get; set; }
    [Parameter] public int LatencyInMilliseconds { get; set; }
    [Parameter] public int OnlinePlayers { get; set; }
    [Inject] private ILocalStorageService LocalStorageService { get; set; }
    [Inject] private IMediator Mediator { get; set; }

    private string _nameError = string.Empty;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            return;
        }

        var playerGuid = await LocalStorageService.GetItemAsync<Guid?>("RPSUserGuid");
        if (playerGuid is not null)
        {
            var player = await Mediator.Send(new GetPlayerDataCommand {PlayerGuid = playerGuid.Value});
            if (player is null)
            {
                await LocalStorageService.RemoveItemAsync("RPSUserGuid");
                return;
            }

            Player = player;
            Player.IsPlayerDataLoaded = true;
            await OnPlayerChanged.InvokeAsync(Player);
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task UpdateUserName(string userName)
    {
        if (Player is not null && Player.IsPlayerDataLoaded) return;

        _nameError = string.Empty;
        if (string.IsNullOrWhiteSpace(userName) || userName.Length is < 3 or > 16)
        {
            _nameError = "Name must be between 3 and 16 characters long.";
            StateHasChanged();
            return;
        }

        var player = new Player
        {
            UserName = userName,
            Guid = Guid.NewGuid(),
            IsPlayerDataLoaded = true
        };

        await OnPlayerChanged.InvokeAsync(player);
        await LocalStorageService.SetItemAsync("RPSUserGuid", player.Guid);
        await Mediator.Publish(new AddPlayerDataNotification {Player = player});
        Player = player;
        StateHasChanged();
    }
}
