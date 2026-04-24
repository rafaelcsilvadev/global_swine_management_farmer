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
            throw new InvalidOperationException("Warehouse with this code already exists");

        var title = await _db.WarehouseTitles.FirstOrDefaultAsync(t => t.Title == payload.Title);
        if (title == null)
        {
            title = new WarehouseTitleEntity { Title = payload.Title };
            _db.WarehouseTitles.Add(title);
        }

        var warehouse = new WarehouseEntity
        {
            Code = payload.Code,
            Title = title
        };

        _db.Warehouses.Add(warehouse);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<WarehouseResponse>> GetAllAsync()
    {
        return await _db.Warehouses
            .Include(w => w.Title)
            .Select(w => new WarehouseResponse(w.Id, w.Code, w.Title.Title))
            .ToListAsync();
    }

    public async Task<WarehouseDto?> GetByIdAsync(Guid id)
    {
        var warehouse = await _db.Warehouses
            .Include(w => w.Title)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (warehouse == null) return null;

        return new WarehouseDto(warehouse.Code, warehouse.Title.Title);
    }

    public async Task UpdateAsync(Guid id, WarehouseDto payload)
    {
        var warehouse = await _db.Warehouses.FindAsync(id)
            ?? throw new KeyNotFoundException("Warehouse not found");

        if (await _db.Warehouses.AnyAsync(w => w.Code == payload.Code && w.Id != id))
            throw new InvalidOperationException("Warehouse with this code already exists");

        var title = await _db.WarehouseTitles.FirstOrDefaultAsync(t => t.Title == payload.Title);
        if (title == null)
        {
            title = new WarehouseTitleEntity { Title = payload.Title };
            _db.WarehouseTitles.Add(title);
        }

        warehouse.Code = payload.Code;
        warehouse.Title = title;
        warehouse.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var warehouse = await _db.Warehouses.FindAsync(id)
            ?? throw new KeyNotFoundException("Warehouse not found");

        warehouse.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }
}
