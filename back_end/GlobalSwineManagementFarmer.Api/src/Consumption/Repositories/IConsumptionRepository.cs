namespace GlobalSwineManagementFarmer.Api.src.Consumption.Repositories;

using GlobalSwineManagementFarmer.Api.src.Consumption.DTOs;

public interface IConsumptionRepository
{
    Task<IEnumerable<ConsumptionResponse>> GetAllAsync();
    Task<ConsumptionResponse?> GetByIdAsync(Guid id);
    Task<ConsumptionResponse> CreateAsync(ConsumptionDto payload);
    Task UpdateAsync(Guid id, ConsumptionDto payload);
    Task DeleteAsync(Guid id);
}
