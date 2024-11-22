using Domain.Entities.Enums;
using Domain.ValueObjects;

namespace Controller.Dtos.OrderResponse;

public record UpdateOrderStatusToReceivedRequest
{
    public PaymentMethod PaymentMethod { get; set; }
}
