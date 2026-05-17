using Microsoft.AspNetCore.Mvc;
using Warehouse.Infrastructure.Dto;

namespace Warehouse.Api.Controllers;

[ApiController]
public class WarehousesController : ControllerBase
{
    [HttpPost("api/warehouses")]
    public IActionResult AddWarehouse([FromBody] WarehouseDto dto, CancellationToken token)
    {
        return Created();
    }
}