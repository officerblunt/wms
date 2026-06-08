using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Warehouse.Api.Interfaces;
using Warehouse.Domain.Event;
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

        var domainEvent = new OrderCreatedDomainEvent(order.Id);

        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = DateTime.UtcNow,
            Type = domainEvent.GetType().FullName ?? "OrderCreatedDomainEvent",
            Content = JsonConvert.SerializeObject(domainEvent),
        };

        context.OutboxMessages.Add(outboxMessage);

        await context.SaveChangesAsync(token);
        
        return true;
    }
}