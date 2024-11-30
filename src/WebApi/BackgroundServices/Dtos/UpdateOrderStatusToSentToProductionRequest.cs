namespace WebApi.BackgroundServices.Dtos;

public record UpdateOrderStatusToSentToProductionRequest
{
    public int OrderId { get; set; }
}
