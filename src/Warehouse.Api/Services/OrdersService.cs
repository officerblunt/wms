using Warehouse.Api.Interfaces;
using Warehouse.Infrastructure.Dto;

namespace Warehouse.Api.Services;

public class OrdersService : IOrderService
{
    private readonly Random _random = new();

    public async Task<bool> CreateOrder(ReserveStockDto dto, CancellationToken token)
    {
        return !EmulateQuantityLimitExceedCheck();
    }

    private bool EmulateQuantityLimitExceedCheck() => _random.Next(0, 1) == 1;
}