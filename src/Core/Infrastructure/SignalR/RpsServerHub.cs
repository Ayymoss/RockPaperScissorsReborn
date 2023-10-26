using MediatR;
using Microsoft.AspNetCore.SignalR;
using RockPaperScissors.Core.Business.Mediatr.Commands;
using RockPaperScissors.Core.Business.Mediatr.Events;
using RockPaperScissors.Core.Domain.Enums;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Infrastructure.SignalR;

public class RpsServerHub(IMediator mediator) : Hub
{
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await mediator.Publish(new RemoveMultiplayerPlayerNotification {ConnectionId = Context.ConnectionId});
        await base.OnDisconnectedAsync(exception);
    }

    public async Task Ping()
    {
        await Clients.Client(Context.ConnectionId).SendAsync(SignalRMethods.Pong.ToString());
    }

    public async Task UpdatePlayer(Player player)
    {
        await mediator.Publish(new UpdateMultiplayerPlayerNotification {ConnectionId = Context.ConnectionId, PlayerGuid = player.Guid});
    }

    public async Task<int> GetOnlinePlayers()
    {
        var count = await mediator.Send(new GetMultiplayerPlayersCountCommand());
        return count;
    }
}
