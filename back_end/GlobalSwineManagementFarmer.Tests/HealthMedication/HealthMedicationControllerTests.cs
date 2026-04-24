namespace GlobalSwineManagementFarmer.Tests.HealthMedication;

using global::GlobalSwineManagementFarmer.Api.src.HealthMedication.Controllers;
using global::GlobalSwineManagementFarmer.Api.src.HealthMedication.DTOs;
using global::GlobalSwineManagementFarmer.Api.src.HealthMedication.Repositories;
using global::GlobalSwineManagementFarmer.Api.src.Symptom.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class HealthMedicationControllerTests
{
    private readonly Mock<IHealthMedicationRepository> _repoMock;
    private readonly HealthMedicationController _controller;

    public HealthMedicationControllerTests()
    {
        _repoMock = new Mock<IHealthMedicationRepository>();
        _controller = new HealthMedicationController(_repoMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithHealthReports()
    {
        var list = new List<HealthMedicationResponse>
        {
            new HealthMedicationResponse(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), null, "Good", new List<SymptomResponse>(), DateTime.UtcNow)
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
        var report = new HealthMedicationResponse(id, Guid.NewGuid(), Guid.NewGuid(), null, "Good", new List<SymptomResponse>(), DateTime.UtcNow);
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(report);

        var result = await _controller.GetById(id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(report, okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenNotExists()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((HealthMedicationResponse?)null);

        var result = await _controller.GetById(id);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ShouldReturnCreated_WhenValid()
    {
        var dto = new HealthMedicationDto(Guid.NewGuid(), Guid.NewGuid(), null, "Good", new List<Guid> { Guid.NewGuid() });
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
        var dto = new HealthMedicationDto(Guid.NewGuid(), Guid.NewGuid(), null, "Excellent", new List<Guid> { Guid.NewGuid() });
        _repoMock.Setup(r => r.UpdateAsync(id, dto)).Returns(Task.CompletedTask);

        var result = await _controller.Update(id, dto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenDoesNotExist()
    {
        var id = Guid.NewGuid();
        var dto = new HealthMedicationDto(Guid.NewGuid(), Guid.NewGuid(), null, "Excellent", new List<Guid> { Guid.NewGuid() });
        _repoMock.Setup(r => r.UpdateAsync(id, dto)).ThrowsAsync(new KeyNotFoundException("Health record not found"));

        var result = await _controller.Update(id, dto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Health record not found", notFoundResult.Value);
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
    public async Task Delete_ShouldReturnNotFound_WhenDoesNotExist()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.DeleteAsync(id)).ThrowsAsync(new KeyNotFoundException("Health record not found"));

        var result = await _controller.Delete(id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Health record not found", notFoundResult.Value);
    }
}
