using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Warehouse.Application.Interfaces.Communication;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Api.Services.Background;

public class OutboxBackgroundWorker(IServiceProvider serviceProvider) : BackgroundService
{
    private const string QueueName = "outbox";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await HandleOutbox(stoppingToken);
            await Task.Delay(500, stoppingToken);
        }
    }

    private async Task HandleOutbox(CancellationToken token = default)
    {
        using var scope = serviceProvider.CreateScope();
        var producer = scope.ServiceProvider.GetRequiredService<IRabbitMqProducer>();
        var context = scope.ServiceProvider.GetRequiredService<WmsContext>();

        var outboxMessagesRecords = await context.OutboxMessages.ToListAsync(cancellationToken: token);
        foreach (var messageRecord in outboxMessagesRecords)
        {
            await producer.PublishMessage(
                JsonConvert.SerializeObject(messageRecord, Formatting.Indented),
                QueueName,
                token
            );
        }
        
        context.OutboxMessages.RemoveRange(outboxMessagesRecords);
        await context.SaveChangesAsync(token);
    }
}