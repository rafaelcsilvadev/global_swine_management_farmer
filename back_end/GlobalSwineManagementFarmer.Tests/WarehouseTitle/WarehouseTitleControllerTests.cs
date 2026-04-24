namespace GlobalSwineManagementFarmer.Tests.WarehouseTitle;

using GlobalSwineManagementFarmer.Api.src.Warehouse.Controllers;
using GlobalSwineManagementFarmer.Api.src.Warehouse.DTOs;
using GlobalSwineManagementFarmer.Api.src.Warehouse.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class WarehouseTitleControllerTests
{
    private readonly Mock<IWarehouseTitleRepository> _repoMock;
    private readonly WarehouseTitleController _controller;

    public WarehouseTitleControllerTests()
    {
        _repoMock = new Mock<IWarehouseTitleRepository>();
        _controller = new WarehouseTitleController(_repoMock.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnCreated_WhenSuccessful()
    {
        var dto = new WarehouseTitleDto(null, "New Title");
        _repoMock.Setup(x => x.CreateAsync(dto)).Returns(Task.CompletedTask);

        var result = await _controller.Create(dto);

        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(201, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        var titles = new List<WarehouseTitleDto> { new WarehouseTitleDto(Guid.NewGuid(), "Title 1") };
        _repoMock.Setup(x => x.GetAllAsync()).ReturnsAsync(titles);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(titles, okResult.Value);
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent_WhenSuccessful()
    {
        var id = Guid.NewGuid();
        var dto = new WarehouseTitleDto(null, "Updated Title");
        _repoMock.Setup(x => x.UpdateAsync(id, dto)).Returns(Task.CompletedTask);

        var result = await _controller.Update(id, dto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenTitleDoesNotExist()
    {
        var id = Guid.NewGuid();
        var dto = new WarehouseTitleDto(null, "Updated Title");
        _repoMock.Setup(x => x.UpdateAsync(id, dto))
            .ThrowsAsync(new KeyNotFoundException("Warehouse title not found"));

        var result = await _controller.Update(id, dto);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenSuccessful()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(x => x.DeleteAsync(id)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(id);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenTitleDoesNotExist()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(x => x.DeleteAsync(id))
            .ThrowsAsync(new KeyNotFoundException("Warehouse title not found"));

        var result = await _controller.Delete(id);

        Assert.IsType<NotFoundObjectResult>(result);
    }
}
