using Microsoft.AspNetCore.SignalR;
using RockPaperScissors.Core.Domain.Enums;
using RockPaperScissors.Core.Domain.Interfaces;
using RockPaperScissors.Core.Infrastructure.SignalR;

namespace RockPaperScissors.Core.Infrastructure.Services;

public class SignalRNotificationService(IHubContext<RpsServerHub> hubContext) : INotificationService
{
    public async Task SendToClient(string connectionId, SignalRMethods methodName, object? data, CancellationToken cancellationToken)
    {
        await hubContext.Clients.Client(connectionId).SendAsync(methodName.ToString(), data);
    }

    public async Task SendToAllClients(SignalRMethods methodName, object? data, CancellationToken cancellationToken)
    {
        await hubContext.Clients.All.SendAsync(methodName.ToString(), data);
    }

    public async Task SendToAllClients(SignalRMethods methodName, CancellationToken cancellationToken)
    {
        await hubContext.Clients.All.SendAsync(methodName.ToString());
    }
}
