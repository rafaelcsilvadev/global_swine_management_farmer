namespace GlobalSwineManagementFarmer.Tests.Warehouse;

using GlobalSwineManagementFarmer.Api.src.Warehouse.Repositories;
using GlobalSwineManagementFarmer.Api.src.Warehouse.DTOs;
using Moq;
using Xunit;

public class WarehouseRepositoryTests
{
    private readonly Mock<IWarehouseRepository> _warehouseRepoMock;

    public WarehouseRepositoryTests()
    {
        _warehouseRepoMock = new Mock<IWarehouseRepository>();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenWarehouseCodeAlreadyExists()
    {
        var dto = new WarehouseDto("W001", "Galpão de Engorda");
        _warehouseRepoMock.Setup(x => x.CreateAsync(dto))
            .ThrowsAsync(new InvalidOperationException("Galpão com este código já existe"));

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _warehouseRepoMock.Object.CreateAsync(dto));
        Assert.Equal("Galpão com este código já existe", ex.Message);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnWarehouse_WhenWarehouseExists()
    {
        var warehouseId = Guid.NewGuid();
        var expectedWarehouse = new WarehouseDto("W001", "Galpão de Engorda");
        _warehouseRepoMock.Setup(x => x.GetByIdAsync(warehouseId))
            .ReturnsAsync(expectedWarehouse);

        var result = await _warehouseRepoMock.Object.GetByIdAsync(warehouseId);

        Assert.NotNull(result);
        Assert.Equal("W001", result.Code);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenWarehouseDoesNotExist()
    {
        var warehouseId = Guid.NewGuid();
        _warehouseRepoMock.Setup(x => x.DeleteAsync(warehouseId))
            .ThrowsAsync(new KeyNotFoundException("Galpão não encontrado"));

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _warehouseRepoMock.Object.DeleteAsync(warehouseId));
    }
}
