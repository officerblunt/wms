using System;
using System.Collections.Generic;

namespace Warehouse.Api;

public partial class StockReservation
{
    public Guid Id { get; set; }

    public Guid OrderItemId { get; set; }

    public Guid WarehouseId { get; set; }

    public Guid LocationId { get; set; }

    public int Quantity { get; set; }

    public Guid IdempotencyKey { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public DateTime? ConsumedAt { get; set; }

    public virtual WarehouseLocation Location { get; set; } = null!;

    public virtual OrderItem OrderItem { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
