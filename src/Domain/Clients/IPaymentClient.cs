using Domain.Entities.OrderAggregate;

namespace Domain.Clients;

public interface IPaymentClient
{
    Task SendAsync(Order order, CancellationToken cancellationToken);
}
