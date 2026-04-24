namespace GlobalSwineManagementFarmer.Api.src.Warehouse.Repositories;

using GlobalSwineManagementFarmer.Api.src.Warehouse.DTOs;

public interface IWarehouseRepository
{
    Task CreateAsync(WarehouseDto payload);
    Task<WarehouseDto?> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, WarehouseDto payload);
    Task DeleteAsync(Guid id);
}
