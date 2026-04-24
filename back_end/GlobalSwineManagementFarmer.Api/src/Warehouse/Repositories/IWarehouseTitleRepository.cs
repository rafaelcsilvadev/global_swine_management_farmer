namespace GlobalSwineManagementFarmer.Api.src.Warehouse.Repositories;

using GlobalSwineManagementFarmer.Api.src.Warehouse.DTOs;

public interface IWarehouseTitleRepository
{
    Task<IEnumerable<WarehouseTitleDto>> GetAllAsync();
    Task CreateAsync(WarehouseTitleDto payload);
    Task UpdateAsync(Guid id, WarehouseTitleDto payload);
    Task DeleteAsync(Guid id);
}
