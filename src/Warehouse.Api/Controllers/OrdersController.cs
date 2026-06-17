using Microsoft.AspNetCore.Mvc;
using Warehouse.Api.Interfaces;
using Warehouse.Infrastructure.Dto;

namespace Warehouse.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ReserveStockDto dto, CancellationToken token)
    {
        if (await orderService.CreateOrder(dto, token)) return Ok();
        return BadRequest("Limit of stock quantity is exceeded");
    }

    [HttpPost("{orderId:guid}/reserve")]
    public async Task<IActionResult> Reserve(Guid orderId, CancellationToken token)
    {
        if (await orderService.ReserveOrder(orderId, token)) return Ok();
        return BadRequest("Failed to reserve order");
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