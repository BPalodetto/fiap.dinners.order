using Domain.Entities.OrderAggregate;
using Domain.ValueObjects;
using UseCase.Dtos.OrderRequest;

namespace UseCase.Services.Interfaces;

public interface IOrderUseCase
{
    Task<Order?> GetAsync(int id, CancellationToken cancellationToken);
    Task<Order?> CreateAsync(CreateOrderRequest orderCreateRequest, CancellationToken cancellationToken);
    Task<Order?> AddProduct(int OrderId, OrderAddProductRequest orderAddProductRequest, CancellationToken cancellationToken);
    Task<Order?> RemoveProduct(int orderId, int productId, CancellationToken cancellationToken);
    Task UpdateStatusToSentToProduction(int orderId, CancellationToken cancellationToken);
    Task UpdateStatusToReceived(int orderId, PaymentMethod paymentMethod, CancellationToken cancellationToken);

}