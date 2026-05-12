using Microsoft.AspNetCore.Mvc;
using Warehouse.Infrastructure.Dto;

namespace Warehouse.Api.Controllers;

[ApiController]
[Route("api/stock")]
public class StockController : ControllerBase
{
    [HttpPost("receive")]
    public IActionResult ReceiveProduct(ProductDto dto)
    {
        return Ok();
    }

    [HttpGet("{productId::guid}")]
    public IActionResult GetProduct(Guid productId)
    {
        var response = new ProductDto()
        {
            Name = "TEST",
            Sku = "TEST",
        };
        return Ok(response);
    }
}