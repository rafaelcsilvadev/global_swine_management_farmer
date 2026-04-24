namespace GlobalSwineManagementFarmer.Api.src.Consumption.Controllers;

using GlobalSwineManagementFarmer.Api.src.Consumption.DTOs;
using GlobalSwineManagementFarmer.Api.src.Consumption.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/consumption")]
[Authorize]
public class ConsumptionController : ControllerBase
{
    private readonly IConsumptionRepository _repo;

    public ConsumptionController(IConsumptionRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    [Authorize(Roles = "admin,farmer")]
    public async Task<IActionResult> GetAll()
    {
        var consumptions = await _repo.GetAllAsync();
        return Ok(consumptions);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "admin,farmer")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var consumption = await _repo.GetByIdAsync(id);
        if (consumption == null) return NotFound();
        return Ok(consumption);
    }

    [HttpPost]
    [Authorize(Roles = "admin,farmer")]
    public async Task<IActionResult> Create([FromBody] ConsumptionDto payload)
    {
        var consumption = await _repo.CreateAsync(payload);
        return CreatedAtAction(nameof(GetById), new { id = consumption.Id }, consumption);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ConsumptionDto payload)
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
