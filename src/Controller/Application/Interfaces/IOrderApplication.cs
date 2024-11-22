using Controller.Dtos.OrderResponse;
using UseCase.Dtos.OrderRequest;

namespace Controller.Application.Interfaces;

public interface IOrderApplication
{
    Task<GetOrListOrderResponse?> GetAsync(int id, CancellationToken cancellationToken);

    Task<CreateOrderResponse?> CreateAsync(CreateOrderRequest orderCreateRequest, CancellationToken cancellationToken);

    Task<OrderUpdateOrderProductResponse?> AddProduct(int OrderId, OrderAddProductRequest orderAddProductRequest,
        CancellationToken cancellationToken);

    Task<OrderUpdateOrderProductResponse?> RemoveProduct(int orderId, int productId,
        CancellationToken cancellationToken);

    Task UpdateStatusToSentToProduction(int orderId, CancellationToken cancellationToken);

    Task UpdateStatusToReceived(int orderId, UpdateOrderStatusToReceivedRequest updateOrderStatusToReceivedRequest, CancellationToken cancellationToken);

}
