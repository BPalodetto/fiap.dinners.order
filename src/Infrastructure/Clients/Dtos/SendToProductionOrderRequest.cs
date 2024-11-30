namespace UseCase.Dtos.OrderRequest;

public class SendToProductionOrderRequest
{
    public int Id { get; init; }
    public IEnumerable<SendToProductionOrderProductRequest> OrderProducts { get; init; } = [];
}
