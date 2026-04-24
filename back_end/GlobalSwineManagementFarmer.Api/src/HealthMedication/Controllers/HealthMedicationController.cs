namespace GlobalSwineManagementFarmer.Api.src.HealthMedication.Controllers;

using GlobalSwineManagementFarmer.Api.src.HealthMedication.DTOs;
using GlobalSwineManagementFarmer.Api.src.HealthMedication.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/health-medication")]
[Authorize]
public class HealthMedicationController : ControllerBase
{
    private readonly IHealthMedicationRepository _repo;

    public HealthMedicationController(IHealthMedicationRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    [Authorize(Roles = "admin,farmer")]
    public async Task<IActionResult> GetAll()
    {
        var reports = await _repo.GetAllAsync();
        return Ok(reports);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "admin,farmer")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var report = await _repo.GetByIdAsync(id);
        if (report == null) return NotFound();
        return Ok(report);
    }

    [HttpPost]
    [Authorize(Roles = "admin,farmer")]
    public async Task<IActionResult> Create([FromBody] HealthMedicationDto payload)
    {
        await _repo.CreateAsync(payload);
        return StatusCode(201);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] HealthMedicationDto payload)
    {
        try
        {
            await _repo.UpdateAsync(id, payload);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
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
            return NotFound(ex.Message);
        }
    }
}
