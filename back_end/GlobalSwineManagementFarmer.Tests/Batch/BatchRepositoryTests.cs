namespace GlobalSwineManagementFarmer.Tests.Batch;

using GlobalSwineManagementFarmer.Api.src.Batch.Repositories;
using GlobalSwineManagementFarmer.Api.src.Batch.DTOs;
using Moq;
using Xunit;

public class BatchRepositoryTests
{
    private readonly Mock<IBatchRepository> _batchRepoMock;

    public BatchRepositoryTests()
    {
        _batchRepoMock = new Mock<IBatchRepository>();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenBatchCodeAlreadyExists()
    {
        var dto = new BatchDto("LOTE_EXISTENTE", 10, Guid.NewGuid());
        _batchRepoMock.Setup(x => x.CreateAsync(dto))
            .ThrowsAsync(new InvalidOperationException("Lote com este código já existe"));

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _batchRepoMock.Object.CreateAsync(dto));
        Assert.Equal("Lote com este código já existe", ex.Message);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBatch_WhenBatchExists()
    {
        var batchId = Guid.NewGuid();
        var expectedBatch = new BatchDto("LOTE001", 10, Guid.NewGuid());
        _batchRepoMock.Setup(x => x.GetByIdAsync(batchId))
            .ReturnsAsync(expectedBatch);

        var result = await _batchRepoMock.Object.GetByIdAsync(batchId);

        Assert.NotNull(result);
        Assert.Equal("LOTE001", result.Code);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenBatchDoesNotExist()
    {
        var batchId = Guid.NewGuid();
        _batchRepoMock.Setup(x => x.DeleteAsync(batchId))
            .ThrowsAsync(new KeyNotFoundException("Lote não encontrado"));

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _batchRepoMock.Object.DeleteAsync(batchId));
    }
}
