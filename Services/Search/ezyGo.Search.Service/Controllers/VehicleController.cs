using ezyGo.Search.Domain.Interfaces;
using ezyGo.Search.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ezyGo.Search.Service.Controllers;

[Route("api/vehicle")]
[ApiController]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;
    private readonly ILogger<VehicleController> _logger;
    public VehicleController(IVehicleService vehicleService, ILogger<VehicleController> logger)
    {
        _vehicleService = vehicleService;
        _logger = logger;
    }

    [HttpGet]
    [Route("search/{vehicleSearchRequest}")]
    public IActionResult SearchVehicles([FromRoute] VehicleSearchRequest vehicleSearchRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var availableVehicles = _vehicleService.SearchVehiclesAsync(vehicleSearchRequest);
            return Ok(availableVehicles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong.");
            return BadRequest(ex.Message);
        }
    }
}
