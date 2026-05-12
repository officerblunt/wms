using Microsoft.AspNetCore.Mvc;
using Warehouse.Infrastructure.Dto;

namespace Warehouse.Api.Controllers;

[ApiController]
public class WarehousesController : ControllerBase
{
    [HttpPost("api/warehouses")]
    public IActionResult AddWareHouse(WarehouseDto dto)
    {
        return Created();
    }
}