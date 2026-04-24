namespace GlobalSwineManagementFarmer.Api.src.Auth.Controllers;

using GlobalSwineManagementFarmer.Api.src.Auth.DTOs;
using GlobalSwineManagementFarmer.Api.src.Auth.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _repo;

    public AuthController(IAuthRepository repo) => _repo = repo;

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInDto payload)
    {
        try
        {
            var response = await _repo.SignInAsync(payload);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "admin")]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthDto payload)
    {
        try
        {
            await _repo.CreateAsync(payload);
            return StatusCode(201);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] AuthDto payload)
    {
        try
        {
            await _repo.UpdateAsync(id, payload);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
