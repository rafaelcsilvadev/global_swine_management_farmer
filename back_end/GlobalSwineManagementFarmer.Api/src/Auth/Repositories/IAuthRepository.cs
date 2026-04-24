namespace GlobalSwineManagementFarmer.Api.src.Auth.Repositories;

using GlobalSwineManagementFarmer.Api.src.Auth.DTOs;

public interface IAuthRepository
{
    Task<AuthResponse> SignInAsync(SignInDto payload);
    Task CreateAsync(AuthDto payload);
    Task UpdateAsync(Guid id, AuthDto payload);
    Task DeleteAsync(Guid id);
}
