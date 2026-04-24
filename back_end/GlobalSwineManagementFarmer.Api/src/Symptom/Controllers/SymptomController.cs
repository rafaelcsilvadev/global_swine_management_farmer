namespace GlobalSwineManagementFarmer.Api.src.Symptom.Controllers;

using GlobalSwineManagementFarmer.Api.src.Symptom.DTOs;
using GlobalSwineManagementFarmer.Api.src.Symptom.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/symptom")]
[Authorize]
public class SymptomController : ControllerBase
{
    private readonly ISymptomRepository _repo;

    public SymptomController(ISymptomRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    [Authorize(Roles = "admin,farmer")]
    public async Task<IActionResult> GetAll()
    {
        var symptoms = await _repo.GetAllAsync();
        return Ok(symptoms);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "admin,farmer")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var symptom = await _repo.GetByIdAsync(id);
        if (symptom == null) return NotFound();
        return Ok(symptom);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] SymptomDto payload)
    {
        try
        {
            await _repo.CreateAsync(payload);
            return StatusCode(201);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SymptomDto payload)
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
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
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
