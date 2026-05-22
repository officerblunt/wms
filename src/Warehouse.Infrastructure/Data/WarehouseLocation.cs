namespace Warehouse.Infrastructure.Data;

public partial class WarehouseLocation
{
    public Guid Id { get; set; }

    public Guid WarehouseId { get; set; }

    public string Code { get; set; } = null!;

    public int Capacity { get; set; }

    public virtual ICollection<StockMovement> StockMovementFromLocations { get; set; } = new List<StockMovement>();

    public virtual ICollection<StockMovement> StockMovementToLocations { get; set; } = new List<StockMovement>();

    public virtual ICollection<StockReservation> StockReservations { get; set; } = new List<StockReservation>();

    public virtual Warehouse Warehouse { get; set; } = null!;
}
