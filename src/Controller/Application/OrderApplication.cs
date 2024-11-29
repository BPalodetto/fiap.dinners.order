using Controller.Application.Interfaces;
using Controller.Dtos.OrderResponse;
using Controller.Extensions.OrderAggregate;
using Domain.ValueObjects;
using UseCase.Dtos.OrderRequest;
using UseCase.Services.Interfaces;

namespace Controller.Application;

public class OrderApplication : IOrderApplication
{
    private readonly IOrderUseCase _orderUseCase;
    public OrderApplication(IOrderUseCase orderUseCase)
    {
        _orderUseCase = orderUseCase;
    }

    public async Task<GetOrListOrderResponse?> GetAsync(int id, CancellationToken cancellationToken)
    {
        var order = await _orderUseCase.GetAsync(id, cancellationToken);

        return
            order?.ToGetOrderResponse();
    }

    public async Task<CreateOrderResponse?> CreateAsync(CreateOrderRequest orderCreateRequest, CancellationToken cancellationToken)
    {
        var order = await _orderUseCase.CreateAsync(orderCreateRequest, cancellationToken);
        if (order == null)
        {
            return null;
        }

        return new()
        {
            OrderId = order.Id
        };
    }

    public async Task<OrderUpdateOrderProductResponse?> AddProduct(int orderId,
        OrderAddProductRequest orderAddProductRequest, CancellationToken cancellationToken)
    {
        var order = await _orderUseCase.AddProduct(orderId, orderAddProductRequest, cancellationToken);

        return
            order?.ToOrderUpdateProductResponse();

    }
    public async Task<OrderUpdateOrderProductResponse?> RemoveProduct(int orderId,
        int productId, CancellationToken cancellationToken)
    {
        var order = await _orderUseCase.RemoveProduct(orderId, productId, cancellationToken);
        return
            order?.ToOrderUpdateProductResponse();
    }

    public async Task UpdateStatusToSentToProduction(int orderId, CancellationToken cancellationToken)
    {
        await _orderUseCase.UpdateStatusToSentToProduction(orderId, cancellationToken);
    }

    public Task UpdateStatusToReceived(int orderId, UpdateOrderStatusToReceivedRequest updateOrderStatusToReceivedRequest, CancellationToken cancellationToken)
    {

        PaymentMethod paymentMethod = new(updateOrderStatusToReceivedRequest.Provider, updateOrderStatusToReceivedRequest.Kind);
        return _orderUseCase.UpdateStatusToReceived(orderId, paymentMethod, cancellationToken);
    }

}
