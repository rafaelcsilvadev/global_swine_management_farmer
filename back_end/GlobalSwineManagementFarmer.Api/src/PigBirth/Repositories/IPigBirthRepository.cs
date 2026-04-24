namespace GlobalSwineManagementFarmer.Api.src.PigBirth.Repositories;

using GlobalSwineManagementFarmer.Api.src.PigBirth.DTOs;

public interface IPigBirthRepository
{
    Task CreateAsync(PigBirthDto payload);
    Task<PigBirthDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<PigBirthResponse>> GetAllAsync();
    Task UpdateAsync(Guid id, PigBirthDto payload);
    Task DeleteAsync(Guid id);
}
