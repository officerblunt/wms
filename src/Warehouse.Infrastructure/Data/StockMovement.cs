using System;
using System.Collections.Generic;

namespace Warehouse.Api;

public partial class StockMovement
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid WarehouseId { get; set; }

    public Guid? FromLocationId { get; set; }

    public Guid? ToLocationId { get; set; }

    public int Quantity { get; set; }

    public string Reason { get; set; } = null!;

    public string ReferenceType { get; set; } = null!;

    public Guid ReferenceId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public virtual WarehouseLocation? FromLocation { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual WarehouseLocation? ToLocation { get; set; }

    public virtual Warehouse Warehouse { get; set; } = null!;
}
