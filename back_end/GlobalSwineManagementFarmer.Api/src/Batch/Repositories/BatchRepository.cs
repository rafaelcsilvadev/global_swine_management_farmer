namespace GlobalSwineManagementFarmer.Api.src.Batch.Repositories;

using GlobalSwineManagementFarmer.Api.Data;
using GlobalSwineManagementFarmer.Api.src.Batch.DTOs;
using GlobalSwineManagementFarmer.Api.src.Batch.Entities;
using Microsoft.EntityFrameworkCore;

public class BatchRepository : IBatchRepository
{
    private readonly AppDbContext _db;

    public BatchRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task CreateAsync(BatchDto payload)
    {
        if (await _db.Batches.AnyAsync(b => b.Code == payload.Code))
            throw new InvalidOperationException("Lote com este código já existe");

        var batch = new BatchEntity
        {
            Code = payload.Code,
            DaysLife = payload.DaysLife,
            WarehouseId = payload.WarehouseId
        };

        _db.Batches.Add(batch);
        await _db.SaveChangesAsync();
    }

    public async Task<BatchDto?> GetByIdAsync(Guid id)
    {
        var batch = await _db.Batches.FindAsync(id);
        if (batch == null) return null;

        return new BatchDto(batch.Code, batch.DaysLife, batch.WarehouseId);
    }

    public async Task UpdateAsync(Guid id, BatchDto payload)
    {
        var batch = await _db.Batches.FindAsync(id)
            ?? throw new KeyNotFoundException("Lote não encontrado");

        batch.Code = payload.Code;
        batch.DaysLife = payload.DaysLife;
        batch.WarehouseId = payload.WarehouseId;
        batch.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var batch = await _db.Batches.FindAsync(id)
            ?? throw new KeyNotFoundException("Lote não encontrado");

        batch.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<BatchDto>> GetAllAsync()
    {
        return await _db.Batches
            .Select(b => new BatchDto(b.Code, b.DaysLife, b.WarehouseId))
            .ToListAsync();
    }
}
