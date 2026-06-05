using Warehouse.Domain.Event;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Api.Services.EventsHandlers;

public class SendMessageHandler : IDomainEventHandler<OrderCreatedDomainEvent>
{
    public Task HandleAsync(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}