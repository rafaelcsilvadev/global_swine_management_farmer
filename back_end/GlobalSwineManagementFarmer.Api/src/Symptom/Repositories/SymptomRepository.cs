namespace GlobalSwineManagementFarmer.Api.src.Symptom.Repositories;

using GlobalSwineManagementFarmer.Api.Data;
using GlobalSwineManagementFarmer.Api.src.Symptom.DTOs;
using GlobalSwineManagementFarmer.Api.src.Symptom.Entities;
using Microsoft.EntityFrameworkCore;

public class SymptomRepository : ISymptomRepository
{
    private readonly AppDbContext _db;

    public SymptomRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<SymptomResponse>> GetAllAsync()
    {
        return await _db.Symptoms
            .Select(s => new SymptomResponse(s.Id, s.Symptom))
            .ToListAsync();
    }

    public async Task<SymptomResponse?> GetByIdAsync(Guid id)
    {
        return await _db.Symptoms
            .Where(s => s.Id == id)
            .Select(s => new SymptomResponse(s.Id, s.Symptom))
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(SymptomDto payload)
    {
        if (await _db.Symptoms.AnyAsync(s => s.Symptom == payload.Symptom))
            throw new InvalidOperationException("Symptom already registered");

        var symptom = new SymptomEntity
        {
            Symptom = payload.Symptom
        };

        _db.Symptoms.Add(symptom);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, SymptomDto payload)
    {
        var symptom = await _db.Symptoms.FindAsync(id)
            ?? throw new KeyNotFoundException("Symptom not found");

        if (await _db.Symptoms.AnyAsync(s => s.Symptom == payload.Symptom && s.Id != id))
            throw new InvalidOperationException("Another symptom with this name already exists");

        symptom.Symptom = payload.Symptom;
        symptom.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var symptom = await _db.Symptoms.FindAsync(id)
            ?? throw new KeyNotFoundException("Symptom not found");

        symptom.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }
}
