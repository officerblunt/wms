using Warehouse.Infrastructure.Dto;

namespace Warehouse.Api.Interfaces;

public interface IProductsService
{
    Task<ProductDto> GetProduct(Guid id, CancellationToken cancellationToken = default);
    Task<ProductDto> GetProduct(string sku, CancellationToken cancellationToken = default);
    Task<bool> CreateProduct(ProductDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateProduct(ProductDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteProduct(Guid id, CancellationToken cancellationToken = default);
}