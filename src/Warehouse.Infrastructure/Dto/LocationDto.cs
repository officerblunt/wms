using Warehouse.Infrastructure.Enum;

namespace Warehouse.Infrastructure.Dto;

public class LocationDto
{
    public Guid Id { get; set; }
    public Guid WarehouseId { get; set; }
    public required string Code { get; set; }
    public WarehouseLocationType Type { get; set; }
    public int Capacity { get; set; }
    public WarehouseLocationStatus Status { get; set; }
}