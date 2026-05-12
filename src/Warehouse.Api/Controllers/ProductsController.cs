using Microsoft.AspNetCore.Mvc;
using Warehouse.Infrastructure.Dto;

namespace Warehouse.Api.Controllers;

[ApiController]
public class ProductsController : ControllerBase
{
    [HttpPost("api/products")]
    public IActionResult AddProducts(ProductDto dto)
    {
        return Created();
    }
}