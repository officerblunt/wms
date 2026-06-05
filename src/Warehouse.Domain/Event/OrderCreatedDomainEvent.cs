using Warehouse.Domain.Interfaces;

namespace Warehouse.Domain.Event;

public class OrderCreatedDomainEvent : IDomainEvent
{
    public DateTime TimeStamp { get; } = DateTime.UtcNow;
    public Guid Id { get; init; }
    public string Content { get; init; }
    public string Type { get; } = typeof(OrderCreatedDomainEvent).FullName ?? string.Empty;
    public string Status { get; set; }
}