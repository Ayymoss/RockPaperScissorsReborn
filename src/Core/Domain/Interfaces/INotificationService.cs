using RockPaperScissors.Core.Domain.Enums;

namespace RockPaperScissors.Core.Domain.Interfaces;

public interface INotificationService
{
    Task SendToClient(string connectionId, SignalRMethods methodName, object? data, CancellationToken cancellationToken);
    Task SendToAllClients(SignalRMethods methodName, object? data, CancellationToken cancellationToken);
    Task SendToAllClients(SignalRMethods methodName, CancellationToken cancellationToken);
}
