using Warehouse.Api.Interfaces;
using Warehouse.Infrastructure.Data;
using Warehouse.Infrastructure.Dto;
using Warehouse.Infrastructure.Enum;

namespace Warehouse.Api.Services.Database;

public class OrdersService(IServiceProvider serviceProvider, IProductsService productsService) : IOrderService
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

    public async Task<bool> CancelOrder(Guid orderId, CancellationToken token)
    {
        var context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<WmsContext>();

        var order = context.Orders.SingleOrDefault(o => o.Id == orderId);
        ArgumentNullException.ThrowIfNull(order);

        try
        {
            if (order is { Status: OrderStatus.Cancelled }) return true;

            order.Status = OrderStatus.Cancelled;
            order.CancelledAt = DateTime.UtcNow;

            context.Orders.Update(order);
            order.Cancel();

            await context.SaveChangesAsync(token);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> PickOrder(Guid orderId, CancellationToken token)
    {
        var context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<WmsContext>();

        var order = context.Orders.SingleOrDefault(o => o.Id == orderId);
        ArgumentNullException.ThrowIfNull(order);

        try
        {
            if (order is { Status: OrderStatus.Picked or OrderStatus.Picking }) return true;

            order.Status = OrderStatus.Picking;

            context.Orders.Update(order);
            order.BeginPickingProcess();

            await context.SaveChangesAsync(token);

            await Task.Delay(1000, token); //physical picking process imitation

            order.Status = OrderStatus.Picked;
            order.PickedAt = DateTime.UtcNow;

            context.Orders.Update(order);
            order.EndPickingProcess();

            await context.SaveChangesAsync(token);
            return true;
        }
        catch
        {
            return false;
        }
    }
}