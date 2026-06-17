using Warehouse.Api.Interfaces;
using Warehouse.Infrastructure.Data;
using Warehouse.Infrastructure.Dto;
using Warehouse.Infrastructure.Enum;

namespace Warehouse.Api.Services;

public class OrdersService(IServiceProvider serviceProvider) : IOrderService
{
    public async Task<bool> CreateOrder(ReserveStockDto dto, CancellationToken token)
    {
        var context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<WmsContext>();

        var order = new Order
        {
            Id = Guid.NewGuid(),
            Status = OrderStatus.Created,
            ExternalOrderId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            OrderItems = []
        };

        context.Orders.Add(order);
        
        order.Create();
        await context.SaveChangesAsync(token);

        return true;
    }

    public async Task<bool> ReserveOrder(Guid orderId, CancellationToken token)
    {
        var context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<WmsContext>();

        var order = context.Orders.SingleOrDefault(o => o.Id == orderId);
        ArgumentNullException.ThrowIfNull(order);

        try
        {
            if (order is { Status: OrderStatus.Reserved }) return true;
            
            order.Status = OrderStatus.Reserved;
            order.ReservedAt = DateTime.UtcNow;
            
            context.Orders.Update(order);
            order.Reserve();
            
            await context.SaveChangesAsync(token);
            return true;
        }
        catch
        {
            return false;
        }
    }
}