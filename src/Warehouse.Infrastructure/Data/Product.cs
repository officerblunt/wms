namespace Warehouse.Infrastructure.Data;

public partial class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Barcode { get; set; }

    public int Weight { get; set; }

    public bool IsActive { get; set; }

    public string Sku { get; set; } = null!;

    public int LengthMm { get; set; }

    public int WidthMm { get; set; }

    public int HeightMm { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<StockBalance> StockBalances { get; set; } = new List<StockBalance>();

    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}
