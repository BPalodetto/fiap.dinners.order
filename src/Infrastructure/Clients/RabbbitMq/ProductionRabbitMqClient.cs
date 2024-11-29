using Domain.Clients;
using Domain.Entities.OrderAggregate;
using RabbitMQ.Client;
using UseCase.Dtos.OrderRequest;

namespace Infrastructure.Clients.RabbbitMq;

public class ProductionRabbitMqClient : RabbitMQPublisher<SendToProductionOrderRequest>, IProductionClient
{
    public const string QueueName = "SendToProduction";
    public ProductionRabbitMqClient(IConnectionFactory factory) : base(factory, QueueName)
    {

    }

    public async Task SendAsync(Order order, CancellationToken cancellationToken)
    {

        List<SendToProductionOrderProductRequest> orderProducts = [];
        foreach (var item in order.OrderProducts)
        {
            SendToProductionOrderProductRequest orderProduct = new()
            {
                Quantity = item.Quantity,
                ProductType = item.Product.ProductType,
                Name = item.Product.Name,
            };
            orderProducts.Add(orderProduct);
        }

        var dto = new SendToProductionOrderRequest
        {
            Id = order.Id,
            OrderProducts = orderProducts
        };

        await PublishMessageAsync(dto, cancellationToken);
    }
}
