using Domain.Entities.Enums;
using Domain.ValueObjects;

namespace Infrastructure.Clients.Dtos;

public record SendPaymentDto
{
    public int OrderId { get; init; }
    public PaymentMethod PaymentMethod { get; init; }
    public decimal Price { get; init; }

}
