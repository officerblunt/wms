using System;
using System.Collections.Generic;

namespace Warehouse.Api;

public partial class Order
{
    public Guid Id { get; set; }

    public Guid? ExternalOrderId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ReservedAt { get; set; }

    public DateTime? PickedAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public DateTime? ShippedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
