namespace GlobalSwineManagementFarmer.Api.src.Warehouse.Repositories;

using GlobalSwineManagementFarmer.Api.Data;
using GlobalSwineManagementFarmer.Api.src.Warehouse.DTOs;
using GlobalSwineManagementFarmer.Api.src.Warehouse.Entities;
using Microsoft.EntityFrameworkCore;

public class WarehouseTitleRepository : IWarehouseTitleRepository
{
    private readonly AppDbContext _db;

    public WarehouseTitleRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<WarehouseTitleDto>> GetAllAsync()
    {
        return await _db.WarehouseTitles
            .Select(wt => new WarehouseTitleDto(wt.Id, wt.Title))
            .ToListAsync();
    }

    public async Task CreateAsync(WarehouseTitleDto payload)
    {
        if (await _db.WarehouseTitles.AnyAsync(wt => wt.Title == payload.Title))
            throw new InvalidOperationException("Warehouse title already exists");

        var entity = new WarehouseTitleEntity
        {
            Title = payload.Title
        };

        _db.WarehouseTitles.Add(entity);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, WarehouseTitleDto payload)
    {
        var entity = await _db.WarehouseTitles.FindAsync(id)
            ?? throw new KeyNotFoundException("Warehouse title not found");

        if (await _db.WarehouseTitles.AnyAsync(wt => wt.Title == payload.Title && wt.Id != id))
            throw new InvalidOperationException("Warehouse title already exists");

        entity.Title = payload.Title;
        entity.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _db.WarehouseTitles.FindAsync(id)
            ?? throw new KeyNotFoundException("Warehouse title not found");

        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }
}
