namespace Warehouse.Infrastructure.Dto;

public class ReserveStockDto
{
    public List<string> Sku { get; set; } = [];
    public int Quantity { get; set; }
    public Guid WarehouseId { get; set; }
}