namespace GlobalSwineManagementFarmer.Api.src.HealthMedication.Repositories;

using GlobalSwineManagementFarmer.Api.src.HealthMedication.DTOs;

public interface IHealthMedicationRepository
{
    Task<IEnumerable<HealthMedicationResponse>> GetAllAsync();
    Task<HealthMedicationResponse?> GetByIdAsync(Guid id);
    Task CreateAsync(HealthMedicationDto payload);
    Task UpdateAsync(Guid id, HealthMedicationDto payload);
    Task DeleteAsync(Guid id);
}
