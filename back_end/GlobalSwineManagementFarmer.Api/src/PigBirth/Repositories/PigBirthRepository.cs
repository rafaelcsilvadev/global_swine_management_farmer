namespace GlobalSwineManagementFarmer.Api.src.PigBirth.Repositories;

using GlobalSwineManagementFarmer.Api.Data;
using GlobalSwineManagementFarmer.Api.src.PigBirth.DTOs;
using GlobalSwineManagementFarmer.Api.src.PigBirth.Entities;
using Microsoft.EntityFrameworkCore;

public class PigBirthRepository : IPigBirthRepository
{
    private readonly AppDbContext _db;

    public PigBirthRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task CreateAsync(PigBirthDto payload)
    {
        if (!await _db.Batches.AnyAsync(b => b.Id == payload.BatchId))
            throw new KeyNotFoundException("Batch not found");

        if (!await _db.Warehouses.AnyAsync(w => w.Id == payload.WarehouseId))
            throw new KeyNotFoundException("Warehouse not found");

        if (!await _db.Batches.AnyAsync(b => b.Id == payload.BatchId && b.WarehouseId == payload.WarehouseId))
            throw new InvalidOperationException("Batch does not belong to this warehouse");

        var pigBirth = new PigBirthEntity
        {
            LivesBirth = payload.LivesBirth,
            StillBorn = payload.StillBorn,
            MummifiedBirth = payload.MummifiedBirth,
            BatchId = payload.BatchId,
            WarehouseId = payload.WarehouseId
        };

        _db.PigBirths.Add(pigBirth);
        await _db.SaveChangesAsync();
    }

    public async Task<PigBirthDto?> GetByIdAsync(Guid id)
    {
        var pb = await _db.PigBirths.FindAsync(id);
        if (pb == null) return null;

        return new PigBirthDto(pb.LivesBirth, pb.StillBorn, pb.MummifiedBirth, pb.BatchId, pb.WarehouseId);
    }

    public async Task<IEnumerable<PigBirthResponse>> GetAllAsync()
    {
        return await _db.PigBirths
            .Select(pb => new PigBirthResponse(pb.Id, pb.LivesBirth, pb.StillBorn, pb.MummifiedBirth, pb.BatchId, pb.WarehouseId))
            .ToListAsync();
    }

    public async Task UpdateAsync(Guid id, PigBirthDto payload)
    {
        var pb = await _db.PigBirths.FindAsync(id)
            ?? throw new KeyNotFoundException("Pig birth record not found");

        pb.LivesBirth = payload.LivesBirth;
        pb.StillBorn = payload.StillBorn;
        pb.MummifiedBirth = payload.MummifiedBirth;
        pb.BatchId = payload.BatchId;
        pb.WarehouseId = payload.WarehouseId;
        pb.UpdatedAt      = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var pb = await _db.PigBirths.FindAsync(id)
            ?? throw new KeyNotFoundException("Pig birth record not found");

        pb.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }
}
