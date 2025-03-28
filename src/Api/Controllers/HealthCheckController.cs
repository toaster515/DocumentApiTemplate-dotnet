namespace Api.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class HealthCheckController : ControllerBase
{
[HttpGet]
public IActionResult Get() => Ok("Healthy");
}
