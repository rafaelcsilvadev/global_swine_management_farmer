namespace GlobalSwineManagementFarmer.Tests.Auth;

using GlobalSwineManagementFarmer.Api.src.Auth.Repositories;
using GlobalSwineManagementFarmer.Api.src.Auth.DTOs;
using Moq;
using Xunit;

public class AuthRepositoryTests
{
    private readonly Mock<IAuthRepository> _authRepoMock;

    public AuthRepositoryTests()
    {
        _authRepoMock = new Mock<IAuthRepository>();
    }

    [Fact]
    public async Task SignInAsync_ShouldThrowUnauthorizedAccessException_WhenCredentialsAreInvalid()
    {
        var payload = new SignInDto("invalid@test.com", "WrongPass1!");
        _authRepoMock.Setup(x => x.SignInAsync(payload))
            .ThrowsAsync(new UnauthorizedAccessException("Email ou senha inválidos"));

        var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _authRepoMock.Object.SignInAsync(payload));
        Assert.Equal("Email ou senha inválidos", ex.Message);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenEmailAlreadyExists()
    {
        var payload = new AuthDto("exists@test.com", "Password123!", Guid.NewGuid());
        _authRepoMock.Setup(x => x.CreateAsync(payload))
            .ThrowsAsync(new InvalidOperationException("Email já cadastrado"));

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _authRepoMock.Object.CreateAsync(payload));
        Assert.Equal("Email já cadastrado", ex.Message);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenRoleIsInvalid()
    {
        var payload = new AuthDto("new@test.com", "Password123!", Guid.NewGuid());
        _authRepoMock.Setup(x => x.CreateAsync(payload))
            .ThrowsAsync(new InvalidOperationException("Role inválida"));

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _authRepoMock.Object.CreateAsync(payload));
        Assert.Equal("Role inválida", ex.Message);
    }

    [Fact]
    public async Task SignInAsync_ShouldHandleSqlInjection_Properly()
    {
        var injectionEmail = "admin' OR '1'='1";
        var payload = new SignInDto(injectionEmail, "Password123!");
        
        _authRepoMock.Setup(x => x.SignInAsync(payload))
            .ThrowsAsync(new UnauthorizedAccessException("Email ou senha inválidos"));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _authRepoMock.Object.SignInAsync(payload));
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenFarmerDoesNotExist()
    {
        var id = Guid.NewGuid();
        var payload = new AuthDto("test@test.com", "Password123!", Guid.NewGuid());
        _authRepoMock.Setup(x => x.UpdateAsync(id, payload))
            .ThrowsAsync(new KeyNotFoundException("Farmer não encontrado"));

        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _authRepoMock.Object.UpdateAsync(id, payload));
        Assert.Equal("Farmer não encontrado", ex.Message);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenFarmerDoesNotExist()
    {
        var id = Guid.NewGuid();
        _authRepoMock.Setup(x => x.DeleteAsync(id))
            .ThrowsAsync(new KeyNotFoundException("Farmer não encontrado"));

        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _authRepoMock.Object.DeleteAsync(id));
        Assert.Equal("Farmer não encontrado", ex.Message);
    }
}
