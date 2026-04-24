namespace GlobalSwineManagementFarmer.Api.src.PigBirth.Controllers;

using GlobalSwineManagementFarmer.Api.src.PigBirth.DTOs;
using GlobalSwineManagementFarmer.Api.src.PigBirth.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/pig-birth")]
[Authorize]
public class PigBirthController : ControllerBase
{
    private readonly IPigBirthRepository _repo;

    public PigBirthController(IPigBirthRepository repo)
    {
        _repo = repo;
    }

    [HttpPost]
    [Authorize(Roles = "admin,farmer")]
    public async Task<IActionResult> Create([FromBody] PigBirthDto payload)
    {
        await _repo.CreateAsync(payload);
        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = "admin,farmer")]
    public async Task<IActionResult> GetAll()
    {
        var list = await _repo.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "admin,farmer")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var pb = await _repo.GetByIdAsync(id);
        if (pb == null) return NotFound();
        return Ok(pb);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PigBirthDto payload)
    {
        try
        {
            await _repo.UpdateAsync(id, payload);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _repo.DeleteAsync(id);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
