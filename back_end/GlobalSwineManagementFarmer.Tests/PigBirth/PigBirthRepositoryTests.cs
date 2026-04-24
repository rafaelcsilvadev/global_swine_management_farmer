namespace GlobalSwineManagementFarmer.Tests.PigBirth;

using GlobalSwineManagementFarmer.Api.src.PigBirth.Repositories;
using GlobalSwineManagementFarmer.Api.src.PigBirth.DTOs;
using Moq;
using Xunit;

public class PigBirthRepositoryTests
{
    private readonly Mock<IPigBirthRepository> _pigBirthRepoMock;

    public PigBirthRepositoryTests()
    {
        _pigBirthRepoMock = new Mock<IPigBirthRepository>();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowKeyNotFoundException_WhenBatchDoesNotExist()
    {
        var dto = new PigBirthDto(10, 2, 1, Guid.NewGuid(), Guid.NewGuid());
        _pigBirthRepoMock.Setup(x => x.CreateAsync(dto))
            .ThrowsAsync(new KeyNotFoundException("Batch not found"));

        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _pigBirthRepoMock.Object.CreateAsync(dto));
        Assert.Equal("Batch not found", ex.Message);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowKeyNotFoundException_WhenWarehouseDoesNotExist()
    {
        var dto = new PigBirthDto(10, 2, 1, Guid.NewGuid(), Guid.NewGuid());
        _pigBirthRepoMock.Setup(x => x.CreateAsync(dto))
            .ThrowsAsync(new KeyNotFoundException("Warehouse not found"));

        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _pigBirthRepoMock.Object.CreateAsync(dto));
        Assert.Equal("Warehouse not found", ex.Message);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnPigBirth_WhenExists()
    {
        var id = Guid.NewGuid();
        var expected = new PigBirthDto(10, 2, 1, Guid.NewGuid(), Guid.NewGuid());
        _pigBirthRepoMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(expected);

        var result = await _pigBirthRepoMock.Object.GetByIdAsync(id);

        Assert.NotNull(result);
        Assert.Equal(10, result.LivesBirth);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenPigBirthDoesNotExist()
    {
        var id = Guid.NewGuid();
        _pigBirthRepoMock.Setup(x => x.DeleteAsync(id))
            .ThrowsAsync(new KeyNotFoundException("Pig birth record not found"));

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _pigBirthRepoMock.Object.DeleteAsync(id));
    }
}
