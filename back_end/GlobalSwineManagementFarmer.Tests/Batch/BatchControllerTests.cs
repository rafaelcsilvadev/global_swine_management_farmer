namespace GlobalSwineManagementFarmer.Tests.Batch;

using GlobalSwineManagementFarmer.Api.src.Batch.DTOs;
using GlobalSwineManagementFarmer.Api.src.Batch.Controllers;
using GlobalSwineManagementFarmer.Api.src.Batch.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class BatchControllerTests
{
    private readonly Mock<IBatchRepository> _repoMock;
    private readonly BatchController _controller;

    public BatchControllerTests()
    {
        _repoMock = new Mock<IBatchRepository>();
        _controller = new BatchController(_repoMock.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnOk_WhenBatchIsCreated()
    {
        var dto = new BatchDto("LOTE001", 10, Guid.NewGuid());

        _repoMock.Setup(r => r.CreateAsync(dto)).Returns(Task.CompletedTask);
        var result = await _controller.Create(dto);
        
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithBatches()
    {
        var batches = new List<BatchDto> { new BatchDto("LOTE001", 10, Guid.NewGuid()) };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(batches);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(batches, okResult.Value);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenBatchIsUpdated()
    {
        var id = Guid.NewGuid();
        var dto = new BatchDto("LOTE001-UPD", 15, Guid.NewGuid());

        _repoMock.Setup(r => r.UpdateAsync(id, dto)).Returns(Task.CompletedTask);
        var result = await _controller.Update(id, dto);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenBatchIsDeleted()
    {
        var id = Guid.NewGuid();

        _repoMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);
        var result = await _controller.Delete(id);

        Assert.IsType<OkResult>(result);
    }
}
