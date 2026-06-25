using Microsoft.AspNetCore.Mvc;
using Warehouse.Api.Interfaces;
using Warehouse.Infrastructure.Dto;

namespace Warehouse.Api.Controllers;

[ApiController]
public class ProductsController(IProductsService productsService) : ControllerBase
{
    [HttpPost("api/products")]
    public async Task<IActionResult> AddProducts([FromBody] ProductDto dto, CancellationToken token)
    {
        if (await productsService.CreateProduct(dto, token)) return Created();
        return BadRequest("Failed to create product");
    }
}