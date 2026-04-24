namespace GlobalSwineManagementFarmer.Api.src.Symptom.Repositories;

using GlobalSwineManagementFarmer.Api.src.Symptom.DTOs;

public interface ISymptomRepository
{
    Task<IEnumerable<SymptomResponse>> GetAllAsync();
    Task<SymptomResponse?> GetByIdAsync(Guid id);
    Task CreateAsync(SymptomDto payload);
    Task UpdateAsync(Guid id, SymptomDto payload);
    Task DeleteAsync(Guid id);
}
