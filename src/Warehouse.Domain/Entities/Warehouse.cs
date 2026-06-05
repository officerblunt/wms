namespace Warehouse.Domain.Entities;

public class Warehouse
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    public virtual ICollection<StockReservation> StockReservations { get; set; } = new List<StockReservation>();

    public virtual ICollection<WarehouseLocation> WarehouseLocations { get; set; } = new List<WarehouseLocation>();
}