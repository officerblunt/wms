namespace Warehouse.Domain.Entities;

public class StockBalance
{
    public Guid ProductId { get; set; }

    public Guid WarehouseId { get; set; }

    public Guid LocationId { get; set; }

    public int QuantityOnHand { get; set; }

    public int QuantityReserved { get; set; }

    public Guid Id { get; set; }

    public virtual Product Product { get; set; } = null!;
}