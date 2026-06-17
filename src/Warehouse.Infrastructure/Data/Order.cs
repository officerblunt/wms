using Warehouse.Domain.Abstractions;
using Warehouse.Domain.Event;
using Warehouse.Infrastructure.Enum;

namespace Warehouse.Infrastructure.Data;

public partial class Order : Entity
{
    public Guid Id { get; set; }

    public Guid? ExternalOrderId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ReservedAt { get; set; }

    public DateTime? PickedAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public DateTime? ShippedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    
    public OrderStatus Status { get; set; }
    
    public void Reserve()
    {
        ReservedAt = DateTime.UtcNow;
        RaiseDomainEvent(new OrderUpdatedDomainEvent
        {
            OrderId = Id,
            Property = "Status",
            NewValue = "Reserved"
        });
    }

    public void Create()
    {
        CreatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new OrderCreatedDomainEvent(Id));
    }
}
