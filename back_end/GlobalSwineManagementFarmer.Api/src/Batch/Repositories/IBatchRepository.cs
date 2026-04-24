namespace GlobalSwineManagementFarmer.Api.src.Batch.Repositories;

using GlobalSwineManagementFarmer.Api.src.Batch.DTOs;

public interface IBatchRepository
{
    Task CreateAsync(BatchDto payload);
    Task<BatchDto?> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, BatchDto payload);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<BatchDto>> GetAllAsync();
}
