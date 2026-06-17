using System.Text;
using RabbitMQ.Client;
using Warehouse.Application.Interfaces.Communication;

namespace Warehouse.Api.Services.Communication;

public class RabbitMqProducer(IChannel channel) : IRabbitMqProducer
{
    public async Task PublishMessage(string message, string queueName, CancellationToken token = default)
    {
        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null, cancellationToken: token);
        
        var amqpMessage = Encoding.UTF8.GetBytes(message);
        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            mandatory: true,
            basicProperties: new BasicProperties { Persistent = true },
            body: amqpMessage, cancellationToken: token);
    }
}