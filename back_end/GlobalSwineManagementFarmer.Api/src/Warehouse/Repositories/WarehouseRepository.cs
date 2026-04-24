namespace GlobalSwineManagementFarmer.Api.src.Warehouse.Repositories;

using GlobalSwineManagementFarmer.Api.Data;
using GlobalSwineManagementFarmer.Api.src.Warehouse.DTOs;
using GlobalSwineManagementFarmer.Api.src.Warehouse.Entities;
using Microsoft.EntityFrameworkCore;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly AppDbContext _db;

    public WarehouseRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task CreateAsync(WarehouseDto payload)
    {
        if (await _db.Warehouses.AnyAsync(w => w.Code == payload.Code))
            throw new InvalidOperationException("Galpão com este código já existe");

        var warehouse = new WarehouseEntity
        {
            Code = payload.Code
        };

        _db.Warehouses.Add(warehouse);
        await _db.SaveChangesAsync();
    }

    public async Task<WarehouseDto?> GetByIdAsync(Guid id)
    {
        var warehouse = await _db.Warehouses.FindAsync(id);
        if (warehouse == null) return null;

        return new WarehouseDto(warehouse.Code, "Título Mock");
    }

    public async Task UpdateAsync(Guid id, WarehouseDto payload)
    {
        var warehouse = await _db.Warehouses.FindAsync(id)
            ?? throw new KeyNotFoundException("Galpão não encontrado");

        if (await _db.Warehouses.AnyAsync(w => w.Code == payload.Code && w.Id != id))
            throw new InvalidOperationException("Galpão com este código já existe");

        warehouse.Code = payload.Code;
        warehouse.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var warehouse = await _db.Warehouses.FindAsync(id)
            ?? throw new KeyNotFoundException("Galpão não encontrado");

        warehouse.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }
}
