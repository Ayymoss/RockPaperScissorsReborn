using Microsoft.AspNetCore.SignalR;
using RockPaperScissors.Models;

namespace RockPaperScissors.Services.Server;

public class RpsServerHub : Hub
{
    public SessionIdInfo GetConnectionId()
    {
        return new SessionIdInfo
        {
            Identity = Context.ConnectionId
        };
    }
}
