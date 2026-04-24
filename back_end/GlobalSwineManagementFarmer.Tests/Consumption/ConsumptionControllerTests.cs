namespace GlobalSwineManagementFarmer.Tests.Consumption;

using global::GlobalSwineManagementFarmer.Api.src.Consumption.Controllers;
using global::GlobalSwineManagementFarmer.Api.src.Consumption.DTOs;
using global::GlobalSwineManagementFarmer.Api.src.Consumption.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class ConsumptionControllerTests
{
    private readonly Mock<IConsumptionRepository> _repoMock;
    private readonly ConsumptionController _controller;

    public ConsumptionControllerTests()
    {
        _repoMock = new Mock<IConsumptionRepository>();
        _controller = new ConsumptionController(_repoMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithConsumptions()
    {
        var list = new List<ConsumptionResponse>
        {
            new ConsumptionResponse(Guid.NewGuid(), 10, 100, Guid.NewGuid(), Guid.NewGuid())
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(list, okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenExists()
    {
        var id = Guid.NewGuid();
        var consumption = new ConsumptionResponse(id, 10, 100, Guid.NewGuid(), Guid.NewGuid());
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(consumption);

        var result = await _controller.GetById(id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(consumption, okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenNotExists()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ConsumptionResponse?)null);

        var result = await _controller.GetById(id);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction_WhenValid()
    {
        var dto = new ConsumptionDto(10, 100, Guid.NewGuid(), Guid.NewGuid());
        var response = new ConsumptionResponse(Guid.NewGuid(), 10, 100, dto.BatchId, dto.WarehouseId);
        _repoMock.Setup(r => r.CreateAsync(dto)).ReturnsAsync(response);

        var result = await _controller.Create(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(response, createdResult.Value);
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent_WhenValid()
    {
        var id = Guid.NewGuid();
        var dto = new ConsumptionDto(15, 120, Guid.NewGuid(), Guid.NewGuid());
        _repoMock.Setup(r => r.UpdateAsync(id, dto)).Returns(Task.CompletedTask);

        var result = await _controller.Update(id, dto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenEntityDoesNotExist()
    {
        var id = Guid.NewGuid();
        var dto = new ConsumptionDto(15, 120, Guid.NewGuid(), Guid.NewGuid());
        _repoMock.Setup(r => r.UpdateAsync(id, dto)).ThrowsAsync(new KeyNotFoundException("Consumption record not found"));

        var result = await _controller.Update(id, dto);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenValid()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(id);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenEntityDoesNotExist()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.DeleteAsync(id)).ThrowsAsync(new KeyNotFoundException("Consumption record not found"));

        var result = await _controller.Delete(id);

        Assert.IsType<NotFoundObjectResult>(result);
    }
}
