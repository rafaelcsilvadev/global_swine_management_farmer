namespace GlobalSwineManagementFarmer.Tests.WarehouseTitle;

using GlobalSwineManagementFarmer.Api.src.Warehouse.Repositories;
using GlobalSwineManagementFarmer.Api.src.Warehouse.DTOs;
using Moq;
using Xunit;

public class WarehouseTitleRepositoryTests
{
    private readonly Mock<IWarehouseTitleRepository> _repoMock;

    public WarehouseTitleRepositoryTests()
    {
        _repoMock = new Mock<IWarehouseTitleRepository>();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenTitleAlreadyExists()
    {
        var dto = new WarehouseTitleDto(null, "Duplicate Title");
        _repoMock.Setup(x => x.CreateAsync(dto))
            .ThrowsAsync(new InvalidOperationException("Warehouse title already exists"));

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _repoMock.Object.CreateAsync(dto));
        Assert.Equal("Warehouse title already exists", ex.Message);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnList()
    {
        var titles = new List<WarehouseTitleDto> { new WarehouseTitleDto(Guid.NewGuid(), "Title 1") };
        _repoMock.Setup(x => x.GetAllAsync()).ReturnsAsync(titles);

        var result = await _repoMock.Object.GetAllAsync();

        Assert.Single(result);
        Assert.Equal("Title 1", result.First().Title);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowInvalidOperationException_WhenTitleAlreadyExists()
    {
        var id = Guid.NewGuid();
        var dto = new WarehouseTitleDto(null, "Existing Title");
        _repoMock.Setup(x => x.UpdateAsync(id, dto))
            .ThrowsAsync(new InvalidOperationException("Warehouse title already exists"));

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _repoMock.Object.UpdateAsync(id, dto));
        Assert.Equal("Warehouse title already exists", ex.Message);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenTitleDoesNotExist()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(x => x.DeleteAsync(id))
            .ThrowsAsync(new KeyNotFoundException("Warehouse title not found"));

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _repoMock.Object.DeleteAsync(id));
    }
}
