namespace GlobalSwineManagementFarmer.Tests.Warehouse;

using GlobalSwineManagementFarmer.Api.src.Warehouse.DTOs;
using GlobalSwineManagementFarmer.Api.src.Warehouse.Controllers;
using GlobalSwineManagementFarmer.Api.src.Warehouse.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class WarehouseControllerTests
{
    private readonly Mock<IWarehouseRepository> _repoMock;
    private readonly WarehouseController _controller;

    public WarehouseControllerTests()
    {
        _repoMock = new Mock<IWarehouseRepository>();
        _controller = new WarehouseController(_repoMock.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnOk_WhenWarehouseIsCreated()
    {
        var dto = new WarehouseDto("W001", "Galpão de Engorda");

        _repoMock.Setup(r => r.CreateAsync(dto)).Returns(Task.CompletedTask);
        var result = await _controller.Create(dto);
        
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenWarehouseExists()
    {
        var id = Guid.NewGuid();
        var dto = new WarehouseDto("W001", "Galpão de Engorda");
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(dto);

        var result = await _controller.GetById(id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dto, okResult.Value);
    }
    [Fact]
    public async Task Update_ShouldReturnOk_WhenWarehouseIsUpdated()
    {
        var id = Guid.NewGuid();
        var dto = new WarehouseDto("W002", "Galpão de Gestação");

        _repoMock.Setup(r => r.UpdateAsync(id, dto)).Returns(Task.CompletedTask);
        var result = await _controller.Update(id, dto);

        Assert.IsType<OkResult>(result);
    }
}
