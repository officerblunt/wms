using Warehouse.Domain.Interfaces;

namespace Warehouse.Domain.Event;

public class OrderUpdatedDomainEvent() : IDomainEvent
{
    public required Guid OrderId { get; set; }
    public DateTime TimeStamp { get; } = DateTime.UtcNow;
    public required string Property { get; init; }
    public required string NewValue { get; init; }
}