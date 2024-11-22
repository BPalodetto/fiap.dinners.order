using Controller.Application.Interfaces;
using Controller.Dtos.OrderResponse;
using Microsoft.AspNetCore.Mvc;
using UseCase.Dtos.OrderRequest;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderApplication _orderApplication;

    public OrderController(IOrderApplication orderApplication)
    {
        _orderApplication = orderApplication;
    }

    [ProducesResponseType(typeof(GetOrListOrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken)
    {
        var response = await _orderApplication.GetAsync(id, cancellationToken);

        if (response == null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [ProducesResponseType(typeof(IEnumerable<CreateOrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderRequest orderCreateRequest, CancellationToken cancellationToken)
    {
        var response = await _orderApplication.CreateAsync(orderCreateRequest, cancellationToken);

        return Ok(response);
    }

    [ProducesResponseType(typeof(IEnumerable<OrderUpdateOrderProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    [Route("{id}/product")]
    public async Task<IActionResult> AddProductAsync(
        int id,
        OrderAddProductRequest orderAddProductRequest,
        CancellationToken cancellationToken)
    {
        var response = await _orderApplication.AddProduct(id, orderAddProductRequest, cancellationToken);

        return Ok(response);
    }

    [ProducesResponseType(typeof(IEnumerable<OrderUpdateOrderProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete]
    [Route("{orderId}/product/{productId}")]
    public async Task<IActionResult> RemoveProductAsync(int orderId, int productId, CancellationToken cancellationToken)
    {
        var response = await _orderApplication.RemoveProduct(orderId, productId, cancellationToken);
        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPatch]
    [Route("{id}/status/received")]
    public async Task<IActionResult> UpdateStatusToReceived(int id, UpdateOrderStatusToReceivedRequest updateOrderStatusToReceivedRequest, CancellationToken cancellationToken)
    {
        await _orderApplication.UpdateStatusToReceived(id, updateOrderStatusToReceivedRequest, cancellationToken);

        return NoContent();
    }
}