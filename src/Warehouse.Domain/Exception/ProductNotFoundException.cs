namespace Warehouse.Domain.Exception;

public class ProductNotFoundException : WmsDomainException
{
    protected ProductNotFoundException(Guid productId) : base($"Product with id {productId} not found", 404)
    {
    }
}