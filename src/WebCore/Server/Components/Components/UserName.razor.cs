using Blazored.LocalStorage;
using MediatR;
using Microsoft.AspNetCore.Components;
using RockPaperScissors.Core.Business.Mediatr.Commands;
using RockPaperScissors.Core.Business.Mediatr.Events;
using RockPaperScissors.Core.Domain.ValueObjects;
using RockPaperScissors.Core.Infrastructure.SignalR;

namespace RockPaperScissors.WebCore.Server.Components.Components;

public partial class UserName
{
    [Inject] private ILocalStorageService LocalStorageService { get; set; }
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private Player Player { get; set; }
    
    // TODO: I don't like Fluxor for this. Seems overcomplicated.
    // The `Player` object needs to be updated in other components. Singleton will do this, but I need a way to tell other components to update.
    // The problem I'm facing is if components load out of order, the late early components won't ever update after the fact.

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
            if (player is null) return;
            Player = player;
            Player.NotifyPlayerDataLoad();
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task UpdateUserName(string userName)
    {
        if (Player.IsPlayerDataLoaded) return;

        _nameError = string.Empty;
        if (string.IsNullOrWhiteSpace(userName) || userName.Length is < 3 or > 16)
        {
            _nameError = "Name must be between 3 and 16 characters long.";
            return;
        }

        Player.UserName = userName;
        Player.Guid = Guid.NewGuid();
        await LocalStorageService.SetItemAsync("RPSUserGuid", Player.Guid);
        await Mediator.Publish(new AddPlayerDataNotification {Player = Player});
        StateHasChanged();
    }
}
