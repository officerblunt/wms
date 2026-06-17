namespace Warehouse.Application.Interfaces.Communication;

public interface IRabbitMqProducer
{
    Task PublishMessage(string message, string queueName, CancellationToken token = default);
}