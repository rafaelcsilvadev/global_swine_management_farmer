namespace GlobalSwineManagementFarmer.Tests.Auth;

using GlobalSwineManagementFarmer.Api.src.Auth.Controllers;
using GlobalSwineManagementFarmer.Api.src.Auth.DTOs;
using GlobalSwineManagementFarmer.Api.src.Auth.Repositories;
using Moq;
using Xunit;

public class AuthControllerTests
{
    private readonly Mock<IAuthRepository> _authRepoMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _authRepoMock = new Mock<IAuthRepository>();
        _controller = new AuthController(_authRepoMock.Object);
    }

    [Fact]
    public async Task SignIn_ShouldReturnOk_WhenCredentialsAreValid()
    {
        var payload = new SignInDto("test@test.com", "Password123!");
        var expectedResponse = new AuthResponse("token", 60);
        _authRepoMock.Setup(x => x.SignInAsync(payload))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.SignIn(payload);

        var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
        Assert.Equal(expectedResponse, okResult.Value);
    }

    [Fact]
    public async Task SignIn_ShouldReturnUnauthorized_WhenRepositoryThrowsUnauthorizedAccessException()
    {
        var payload = new SignInDto("test@test.com", "WrongPass!");
        _authRepoMock.Setup(x => x.SignInAsync(payload))
            .ThrowsAsync(new UnauthorizedAccessException("Email ou senha inválidos"));

        var result = await _controller.SignIn(payload);

        Assert.IsType<Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task Register_ShouldReturnCreated_WhenSuccessful()
    {
        var payload = new AuthDto("new@test.com", "Password123!", Guid.NewGuid());
        _authRepoMock.Setup(x => x.CreateAsync(payload)).Returns(Task.CompletedTask);

        var result = await _controller.Register(payload);

        var statusCodeResult = Assert.IsType<Microsoft.AspNetCore.Mvc.StatusCodeResult>(result);
        Assert.Equal(201, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent_WhenSuccessful()
    {
        var id = Guid.NewGuid();
        var payload = new AuthDto("update@test.com", "NewPass123!", Guid.NewGuid());
        _authRepoMock.Setup(x => x.UpdateAsync(id, payload)).Returns(Task.CompletedTask);

        var result = await _controller.Update(id, payload);

        Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenFarmerDoesNotExist()
    {
        var id = Guid.NewGuid();
        var payload = new AuthDto("update@test.com", "NewPass123!", Guid.NewGuid());
        _authRepoMock.Setup(x => x.UpdateAsync(id, payload))
            .ThrowsAsync(new KeyNotFoundException("Farmer não encontrado"));

        var result = await _controller.Update(id, payload);

        Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenSuccessful()
    {
        var id = Guid.NewGuid();
        _authRepoMock.Setup(x => x.DeleteAsync(id)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(id);

        Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenFarmerDoesNotExist()
    {
        var id = Guid.NewGuid();
        _authRepoMock.Setup(x => x.DeleteAsync(id))
            .ThrowsAsync(new KeyNotFoundException("Farmer não encontrado"));

        var result = await _controller.Delete(id);

        Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(result);
    }
}
