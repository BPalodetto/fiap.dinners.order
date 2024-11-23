using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Clients.RabbbitMq;

public abstract class RabbitMQPublisher<T>
{
    private readonly IConnectionFactory _factory;
    private readonly string _queue;
    protected RabbitMQPublisher(IConnectionFactory factory, string queue)
    {
        _factory = factory;
        _queue = queue;
    }

    protected async Task PublishMessageAsync(T message, CancellationToken cancellationToken)
    {
        using var connection = await _factory.CreateConnectionAsync(cancellationToken);
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: _queue, durable: false, exclusive: false, autoDelete: false,
            arguments: null);

        string jsonString = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonString);

        await channel.BasicPublishAsync(string.Empty, _queue, body, cancellationToken);

    }
}
