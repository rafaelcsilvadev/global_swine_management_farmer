namespace GlobalSwineManagementFarmer.Tests.PigBirth;

using GlobalSwineManagementFarmer.Api.src.PigBirth.DTOs;
using GlobalSwineManagementFarmer.Api.src.PigBirth.Controllers;
using GlobalSwineManagementFarmer.Api.src.PigBirth.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class PigBirthControllerTests
{
    private readonly Mock<IPigBirthRepository> _repoMock;
    private readonly PigBirthController _controller;

    public PigBirthControllerTests()
    {
        _repoMock = new Mock<IPigBirthRepository>();
        _controller = new PigBirthController(_repoMock.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnOk_WhenPigBirthIsCreated()
    {
        var dto = new PigBirthDto(10, 2, 1, Guid.NewGuid(), Guid.NewGuid());

        _repoMock.Setup(r => r.CreateAsync(dto)).Returns(Task.CompletedTask);
        var result = await _controller.Create(dto);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithPigBirths()
    {
        var list = new List<PigBirthResponse>
        {
            new PigBirthResponse(Guid.NewGuid(), 10, 2, 1, Guid.NewGuid(), Guid.NewGuid())
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(list, okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenPigBirthExists()
    {
        var id = Guid.NewGuid();
        var dto = new PigBirthDto(10, 2, 1, Guid.NewGuid(), Guid.NewGuid());
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(dto);

        var result = await _controller.GetById(id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dto, okResult.Value);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenPigBirthIsUpdated()
    {
        var id = Guid.NewGuid();
        var dto = new PigBirthDto(12, 3, 0, Guid.NewGuid(), Guid.NewGuid());

        _repoMock.Setup(r => r.UpdateAsync(id, dto)).Returns(Task.CompletedTask);
        var result = await _controller.Update(id, dto);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenPigBirthIsDeleted()
    {
        var id = Guid.NewGuid();

        _repoMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);
        var result = await _controller.Delete(id);

        Assert.IsType<OkResult>(result);
    }
}
