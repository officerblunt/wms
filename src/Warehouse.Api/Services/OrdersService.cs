using Warehouse.Api.Interfaces;
using Warehouse.Infrastructure.Dto;

namespace Warehouse.Api.Services;

public class OrdersService : IOrderService
{
    public async Task<bool> CreateOrder(ReserveStockDto dto, CancellationToken token)
    {
        return !EmulateQuantityLimitExceedCheck();
    }

    private static bool EmulateQuantityLimitExceedCheck() => false;
}