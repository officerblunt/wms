using Microsoft.AspNetCore.Mvc;
using Warehouse.Infrastructure.Dto;

namespace Warehouse.Api.Controllers;

[ApiController]
public class WarehouseLocationsController : ControllerBase
{
    [HttpPost("api/locations")]
    public IActionResult AddLocation([FromBody] LocationDto dto, CancellationToken token)
    {
        return Created();
    }
}