using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Warehouse.Infrastructure.Dto;

namespace Warehouse.Api.Tests.Integration;

public class ReserveOrderTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Post_WithValidData_ShouldReturnOk()
    {
        var dto = new ReserveStockDto
        {
            Sku = ["WMS-12345"],
            Quantity = 10,
            WarehouseId = Guid.NewGuid(),
        };

        var response = await _client.PostAsJsonAsync("api/orders", dto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Post_WithInvalidSku_ShouldReturnBadRequest()
    {
        var dto = new ReserveStockDto
        {
            Sku = ["WMS-asdf"],
            Quantity = 10,
            WarehouseId = Guid.NewGuid(),
        };

        var response = await _client.PostAsJsonAsync("api/orders", dto);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_WithEmptySku_ShouldReturnBadRequest()
    {
        var dto = new ReserveStockDto
        {
            Quantity = 10,
            WarehouseId = Guid.NewGuid(),
        };

        var response = await _client.PostAsJsonAsync("api/orders", dto);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_WithZeroQuantity_ShouldReturnBadRequest()
    {
        var dto = new ReserveStockDto
        {
            Sku = ["WMS-12345"],
            Quantity = 0,
            WarehouseId = Guid.NewGuid(),
        };

        var response = await _client.PostAsJsonAsync("api/orders", dto);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_WithNegativeQuantity_ShouldReturnBadRequest()
    {
        var dto = new ReserveStockDto
        {
            Sku = ["WMS-12345"],
            Quantity = -5,
            WarehouseId = Guid.NewGuid(),
        };

        var response = await _client.PostAsJsonAsync("api/orders", dto);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}