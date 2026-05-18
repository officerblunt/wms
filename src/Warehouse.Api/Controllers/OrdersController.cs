using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(CancellationToken token)
    {
        return Ok();
    }

    [HttpPost("{orderId:guid}/reserve")]
    public async Task<IActionResult> Reserve(Guid orderId, CancellationToken token)
    {
        return Ok();
    }
    
    [HttpPost("{orderId:guid}/cancel-reservation")]
    public async Task<IActionResult> CancelReservation(Guid orderId, CancellationToken token)
    {
        return Ok();
    }
    
    [HttpPost("{orderId:guid}/pick")]
    public async Task<IActionResult> Pick(Guid orderId, CancellationToken token)
    {
        return Ok();
    }
}