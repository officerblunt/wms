using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    [HttpPost]
    public IActionResult Post(CancellationToken token)
    {
        return Ok();
    }

    [HttpPost("{orderId:guid}/reserve")]
    public IActionResult Reserve(Guid orderId, CancellationToken token)
    {
        return Ok();
    }
    
    [HttpPost("{orderId:guid}/cancel-reservation")]
    public IActionResult CancelReservation(Guid orderId, CancellationToken token)
    {
        return Ok();
    }
    
    [HttpPost("{orderId:guid}/pick")]
    public IActionResult Pick(Guid orderId, CancellationToken token)
    {
        return Ok();
    }
}