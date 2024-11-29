using Domain.Entities.Enums;

namespace Controller.Dtos.OrderResponse;

public record UpdateOrderStatusToReceivedRequest
{
    public PaymentProvider Provider { get; set; }
    public PaymentMethodKind Kind { get; set; }

}
