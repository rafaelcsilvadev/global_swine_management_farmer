namespace GlobalSwineManagementFarmer.Api.src.Consumption.Repositories;

using GlobalSwineManagementFarmer.Api.Data;
using GlobalSwineManagementFarmer.Api.src.Consumption.DTOs;
using GlobalSwineManagementFarmer.Api.src.Consumption.Entities;
using Microsoft.EntityFrameworkCore;

public class ConsumptionRepository : IConsumptionRepository
{
    private readonly AppDbContext _db;

    public ConsumptionRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ConsumptionResponse>> GetAllAsync()
    {
        return await _db.Consumptions
            .Select(c => new ConsumptionResponse(c.Id, c.BagPetFood, c.WaterTank, c.BatchId, c.WarehouseId))
            .ToListAsync();
    }

    public async Task<ConsumptionResponse?> GetByIdAsync(Guid id)
    {
        return await _db.Consumptions
            .Where(c => c.Id == id)
            .Select(c => new ConsumptionResponse(c.Id, c.BagPetFood, c.WaterTank, c.BatchId, c.WarehouseId))
            .FirstOrDefaultAsync();
    }

    public async Task<ConsumptionResponse> CreateAsync(ConsumptionDto payload)
    {
        if (!await _db.Batches.AnyAsync(b => b.Id == payload.BatchId))
            throw new KeyNotFoundException("Batch not found");

        if (!await _db.Warehouses.AnyAsync(w => w.Id == payload.WarehouseId))
            throw new KeyNotFoundException("Warehouse not found");

        if (!await _db.Batches.AnyAsync(b => b.Id == payload.BatchId && b.WarehouseId == payload.WarehouseId))
            throw new InvalidOperationException("Batch does not belong to this warehouse");

        var consumption = new ConsumptionEntity
        {
            BagPetFood = payload.BagPetFood,
            WaterTank = payload.WaterTank,
            BatchId = payload.BatchId,
            WarehouseId = payload.WarehouseId
        };

        _db.Consumptions.Add(consumption);
        await _db.SaveChangesAsync();

        return new ConsumptionResponse(consumption.Id, consumption.BagPetFood, consumption.WaterTank, consumption.BatchId, consumption.WarehouseId);
    }

    public async Task UpdateAsync(Guid id, ConsumptionDto payload)
    {
        var consumption = await _db.Consumptions.FindAsync(id)
            ?? throw new KeyNotFoundException("Consumption record not found");

        if (!await _db.Batches.AnyAsync(b => b.Id == payload.BatchId))
            throw new KeyNotFoundException("Batch not found");

        if (!await _db.Warehouses.AnyAsync(w => w.Id == payload.WarehouseId))
            throw new KeyNotFoundException("Warehouse not found");

        if (!await _db.Batches.AnyAsync(b => b.Id == payload.BatchId && b.WarehouseId == payload.WarehouseId))
            throw new InvalidOperationException("Batch does not belong to this warehouse");

        consumption.BagPetFood = payload.BagPetFood;
        consumption.WaterTank = payload.WaterTank;
        consumption.BatchId = payload.BatchId;
        consumption.WarehouseId = payload.WarehouseId;
        consumption.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var consumption = await _db.Consumptions.FindAsync(id)
            ?? throw new KeyNotFoundException("Consumption record not found");

        consumption.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }
}
