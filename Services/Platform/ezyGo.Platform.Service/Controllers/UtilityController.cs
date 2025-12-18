using Microsoft.AspNetCore.Mvc;

namespace ezyGo.Platform.Service.Controllers;

[Route("api/utility")]
[ApiController]
public class UtilityController : ControllerBase
{

    private readonly ILogger<UtilityController> _logger;
}
