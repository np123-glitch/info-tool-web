using Microsoft.AspNetCore.Mvc;

namespace ZmaReference.Features.Healthcheck.UI.Pages;

[ApiController]
[Route("api/v1/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult HealthCheck()
    {
        return Ok();
    }
}