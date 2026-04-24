namespace GlobalSwineManagementFarmer.Api.src.Warehouse.Controllers;

using GlobalSwineManagementFarmer.Api.src.Warehouse.DTOs;
using GlobalSwineManagementFarmer.Api.src.Warehouse.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/warehouse-titles")]
[Authorize]
public class WarehouseTitleController : ControllerBase
{
    private readonly IWarehouseTitleRepository _repo;

    public WarehouseTitleController(IWarehouseTitleRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    [Authorize(Roles = "admin,farmer")]
    public async Task<IActionResult> GetAll()
    {
        var titles = await _repo.GetAllAsync();
        return Ok(titles);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] WarehouseTitleDto payload)
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

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] WarehouseTitleDto payload)
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
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
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
    }
}
