using Microsoft.AspNetCore.SignalR;
using RockPaperScissors.Core.Domain.ValueObjects;

namespace RockPaperScissors.Core.Infrastructure.SignalR;

public class RpsServerHub : Hub
{
    public SessionIdInfoSignalR GetConnectionId()
    {
        return new SessionIdInfoSignalR
        {
            Identity = Context.ConnectionId
        };
    }
}
