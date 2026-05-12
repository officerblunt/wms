namespace Warehouse.Infrastructure.Dto;

public class ProductDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Barcode { get; set; }
    public int Weight { get; set; }
    public bool IsActive { get; set; }
    public required string Sku { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}