namespace Warehouse.Infrastructure.Data;

public partial class OrderItem
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public int OrderedQuantity { get; set; }

    public int ReservedQuantity { get; set; }

    public int PickedQuantity { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<StockReservation> StockReservations { get; set; } = new List<StockReservation>();
}
