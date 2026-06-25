using Warehouse.Infrastructure.Data;
using Warehouse.Infrastructure.Dto;

namespace Warehouse.Api.Extensions;

public static class ProductExtensions
{
    extension(Product p)
    {
        internal ProductDto ToDto() => new()
        {
            Id = p.Id,
            Sku = p.Sku,
            Name = p.Name,
            Barcode = p.Barcode,
            Weight = p.Weight,
            IsActive = p.IsActive,
            Length = p.LengthMm,
            Width = p.WidthMm,
            Height = p.HeightMm,
        };
    }
}