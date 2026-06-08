using Warehouse.Domain.Interfaces;

namespace Warehouse.Domain.Event;

public class OrderCreatedDomainEvent(Guid orderId) : IDomainEvent
{
    public DateTime TimeStamp { get; } = DateTime.UtcNow;
    public Guid OrderId { get; init; } = orderId;
}