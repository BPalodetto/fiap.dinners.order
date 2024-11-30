using Domain.Entities.OrderAggregate;

namespace Domain.Clients;

public interface IProductionClient
{
    Task SendAsync(Order order, CancellationToken cancellationToken);
}
