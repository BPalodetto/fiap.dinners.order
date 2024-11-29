using Domain.Entities.Enums;

namespace UseCase.Dtos.OrderRequest;

public record SendToProductionOrderProductRequest
{

    public int Quantity { get; init; }
    public ProductType ProductType { get; init; }
    public string Name { get; init; } = string.Empty; 
}
