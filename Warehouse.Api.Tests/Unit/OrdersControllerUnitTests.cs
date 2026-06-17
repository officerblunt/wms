using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Warehouse.Api.Controllers;
using Warehouse.Api.Interfaces;
using Warehouse.Infrastructure.Dto;

namespace Warehouse.Api.Tests.Unit;

public class OrdersControllerUnitTests
{
    private readonly IOrderService _orderServiceMock;
    private readonly OrdersController _controller;
    
    public OrdersControllerUnitTests()
    {
        _orderServiceMock = Substitute.For<IOrderService>();
        _controller = new(_orderServiceMock);
    }

    [Fact]
    public async Task Post_WhenServiceSucceeds_ReturnsOkResult()
    {
        var dto = new ReserveStockDto { Sku = ["WMS-55555"], Quantity = 1, WarehouseId = Guid.NewGuid() };
        var token = CancellationToken.None;

        _orderServiceMock.CreateOrder(dto, token).Returns(true);

        var result = await _controller.Post(dto, token);

        result.Should().BeOfType<OkResult>();
        
        await _orderServiceMock.Received(1).CreateOrder(dto, token);
    }
    
    [Fact]
    public async Task Post_WhenServiceFails_ReturnsBadRequest()
    {
        var dto = new ReserveStockDto { Sku = ["asdf"], Quantity = 1, WarehouseId = Guid.NewGuid() };
        var token = CancellationToken.None;

        _orderServiceMock.CreateOrder(dto, token).Returns(false);

        var result = await _controller.Post(dto, token);

        result.Should().BeOfType<BadRequestObjectResult>();
        
        await _orderServiceMock.Received(1).CreateOrder(dto, token);
    }
}