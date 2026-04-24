namespace GlobalSwineManagementFarmer.Api.src.Batch.Controllers;

using GlobalSwineManagementFarmer.Api.src.Batch.DTOs;
using GlobalSwineManagementFarmer.Api.src.Batch.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/batch")]
[Authorize]
public class BatchController : ControllerBase
{
    private readonly IBatchRepository _repo;

    public BatchController(IBatchRepository repo)
    {
        _repo = repo;
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] BatchDto payload)
    {
        await _repo.CreateAsync(payload);
        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = "admin,farmer")]
    public async Task<IActionResult> GetAll()
    {
        var batches = await _repo.GetAllAsync();
        return Ok(batches);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] BatchDto payload)
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
