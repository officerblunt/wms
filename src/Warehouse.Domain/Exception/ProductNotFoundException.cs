namespace Warehouse.Domain.Exception;

public class ProductNotFoundException : WmsDomainException
{
    public ProductNotFoundException(Guid productId) : base($"Product with id {productId} not found", 404)
    {
    }

    public ProductNotFoundException(string sku) : base($"Product with id {sku} not found", 404)
    {
    }
}