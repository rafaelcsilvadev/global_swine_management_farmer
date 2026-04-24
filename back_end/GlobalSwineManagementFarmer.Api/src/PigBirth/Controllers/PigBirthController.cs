namespace GlobalSwineManagementFarmer.Api.src.PigBirth.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/pig-birth")]
public class PigBirthController : ControllerBase
{
    [HttpPost]
    public IActionResult Register()
    {
        return Ok("Birth registered");
    }
}
