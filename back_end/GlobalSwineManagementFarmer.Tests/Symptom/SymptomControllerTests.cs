namespace GlobalSwineManagementFarmer.Tests.Symptom;

using global::GlobalSwineManagementFarmer.Api.src.Symptom.Controllers;
using global::GlobalSwineManagementFarmer.Api.src.Symptom.DTOs;
using global::GlobalSwineManagementFarmer.Api.src.Symptom.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SymptomControllerTests
{
    private readonly Mock<ISymptomRepository> _repoMock;
    private readonly SymptomController _controller;

    public SymptomControllerTests()
    {
        _repoMock = new Mock<ISymptomRepository>();
        _controller = new SymptomController(_repoMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithSymptoms()
    {
        var list = new List<SymptomResponse>
        {
            new SymptomResponse(Guid.NewGuid(), "Fever")
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
        var symptom = new SymptomResponse(id, "Fever");
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(symptom);

        var result = await _controller.GetById(id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(symptom, okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenNotExists()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((SymptomResponse?)null);

        var result = await _controller.GetById(id);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ShouldReturnCreated_WhenValid()
    {
        var dto = new SymptomDto("Cough");
        _repoMock.Setup(r => r.CreateAsync(dto)).Returns(Task.CompletedTask);

        var result = await _controller.Create(dto);

        Assert.IsType<StatusCodeResult>(result);
        var statusCodeResult = result as StatusCodeResult;
        Assert.Equal(201, statusCodeResult?.StatusCode);
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent_WhenValid()
    {
        var id = Guid.NewGuid();
        var dto = new SymptomDto("Persistent cough");
        _repoMock.Setup(r => r.UpdateAsync(id, dto)).Returns(Task.CompletedTask);

        var result = await _controller.Update(id, dto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenSymptomDoesNotExist()
    {
        var id = Guid.NewGuid();
        var dto = new SymptomDto("Persistent cough");
        _repoMock.Setup(r => r.UpdateAsync(id, dto)).ThrowsAsync(new KeyNotFoundException("Symptom not found"));

        var result = await _controller.Update(id, dto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Symptom not found", notFoundResult.Value);
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
    public async Task Delete_ShouldReturnNotFound_WhenSymptomDoesNotExist()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.DeleteAsync(id)).ThrowsAsync(new KeyNotFoundException("Symptom not found"));

        var result = await _controller.Delete(id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Symptom not found", notFoundResult.Value);
    }
}
