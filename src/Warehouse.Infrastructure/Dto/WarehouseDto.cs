using Warehouse.Infrastructure.Enum;

namespace Warehouse.Infrastructure.Dto;

public class WarehouseDto
{
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public WarehouseStatus Status { get; set; }
}