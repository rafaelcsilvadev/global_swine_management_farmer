namespace GlobalSwineManagementFarmer.Api.src.HealthMedication.Repositories;

using GlobalSwineManagementFarmer.Api.Data;
using GlobalSwineManagementFarmer.Api.src.HealthMedication.DTOs;
using GlobalSwineManagementFarmer.Api.src.HealthMedication.Entities;
using GlobalSwineManagementFarmer.Api.src.Symptom.Entities;
using GlobalSwineManagementFarmer.Api.src.Symptom.DTOs;
using Microsoft.EntityFrameworkCore;

public class HealthMedicationRepository : IHealthMedicationRepository
{
    private readonly AppDbContext _db;

    public HealthMedicationRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<HealthMedicationResponse>> GetAllAsync()
    {
        return await _db.HealthMedications
            .Select(hm => new HealthMedicationResponse(
                hm.Id,
                hm.BatchId,
                hm.WarehouseId,
                hm.Media,
                hm.TreatmentEfficacy,
                hm.SymptomsObserved.Select(so => new SymptomResponse(
                    so.Symptom.Id,
                    so.Symptom.Symptom
                )).ToList(),
                hm.CreatedAt
            ))
            .ToListAsync();
    }

    public async Task<HealthMedicationResponse?> GetByIdAsync(Guid id)
    {
        return await _db.HealthMedications
            .Where(hm => hm.Id == id)
            .Select(hm => new HealthMedicationResponse(
                hm.Id,
                hm.BatchId,
                hm.WarehouseId,
                hm.Media,
                hm.TreatmentEfficacy,
                hm.SymptomsObserved.Select(so => new SymptomResponse(
                    so.Symptom.Id,
                    so.Symptom.Symptom
                )).ToList(),
                hm.CreatedAt
            ))
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(HealthMedicationDto payload)
    {
        var healthMedication = new HealthMedicationEntity
        {
            BatchId = payload.BatchId,
            WarehouseId = payload.WarehouseId,
            Media = payload.Media,
            TreatmentEfficacy = payload.TreatmentEfficacy,
            SymptomsObserved = payload.SymptomIds.Select(symptomId => new SymptomObservedEntity
            {
                SymptomId = symptomId
            }).ToList()
        };

        _db.HealthMedications.Add(healthMedication);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, HealthMedicationDto payload)
    {
        var healthMedication = await _db.HealthMedications
            .Include(hm => hm.SymptomsObserved)
            .FirstOrDefaultAsync(hm => hm.Id == id)
            ?? throw new KeyNotFoundException("Health record not found");

        healthMedication.BatchId = payload.BatchId;
        healthMedication.WarehouseId = payload.WarehouseId;
        healthMedication.Media = payload.Media;
        healthMedication.TreatmentEfficacy = payload.TreatmentEfficacy;
        healthMedication.UpdatedAt = DateTime.UtcNow;

        _db.SymptomsObserved.RemoveRange(healthMedication.SymptomsObserved);

        healthMedication.SymptomsObserved = payload.SymptomIds.Select(symptomId => new SymptomObservedEntity
        {
            HealthMedicationId = id,
            SymptomId = symptomId
        }).ToList();

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var healthMedication = await _db.HealthMedications.FindAsync(id)
            ?? throw new KeyNotFoundException("Health record not found");

        healthMedication.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }
}
