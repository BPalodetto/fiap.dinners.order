using Domain.Clients;
using Domain.Entities.OrderAggregate;
using Infrastructure.Clients.Dtos;
using RabbitMQ.Client;

namespace Infrastructure.Clients.RabbbitMq;

public class PaymentRabbitMqClient :  RabbitMQPublisher<SendPaymentDto>, IPaymentClient
{
    public const string QueueName = "SendToPayment";
    public PaymentRabbitMqClient(IConnectionFactory factory) : base(factory, QueueName)
    {

    }

    public async Task SendAsync(Order order, CancellationToken cancellationToken)
    {
        var dto = new SendPaymentDto
        {
            OrderId = order.Id,
            PaymentMethod = order.PaymentMethod!.Value,
            Price = order.Price
        };

        await PublishMessageAsync(dto, cancellationToken);
    }


}
