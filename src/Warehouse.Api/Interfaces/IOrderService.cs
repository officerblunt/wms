using Warehouse.Infrastructure.Dto;

namespace Warehouse.Api.Interfaces;

public interface IOrderService
{
    Task<bool> CreateOrder(ReserveStockDto dto, CancellationToken token);
    Task<bool> ReserveOrder(Guid orderId, CancellationToken token);
}