using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ezyGo.Admin.Service.Controllers;

[Route("api/route")]
[ApiController]
public class RouteController : ControllerBase
{
    private readonly IRouteService _routeService;
    private readonly ILogger<RouteController> _logger;

    public RouteController(IRouteService routeService, ILogger<RouteController> logger)
    {
        _routeService = routeService;
        _logger = logger;
    }


    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreateRoute([FromBody] Domain.Models.Route route)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdRoute =  await _routeService.CreateRouteAsync(route);
            return Ok(createdRoute);
        }
        catch (AlreadyExistException ex)
        {
            _logger.LogInformation(ex, "This route is already exist");
            return Ok(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating route");
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> UpdateRoute([FromBody] Domain.Models.Route route)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _routeService.UpdateRouteAsync(route);
            return Ok(route);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating route");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetRouteById(int id)
    {
        try
        {
            var route = await _routeService.GetRouteByIdAsync(id);
            return Ok(route);
        }
        catch (NotFoundException ex)
        {
            _logger.LogInformation($"Route not found: {ex.Message}");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching route by ID");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetRoutes(string? filter)
    {
        try
        {
            var routes = await _routeService.GetRoutesAsync(filter);
            return Ok(routes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching routes");
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteRoute(int id)
    {
        try
        {
            await _routeService.DeleteRouteAsync(id);
            return Ok($"Route with id {id} deleted successfully");
        }
        catch (NotFoundException ex)
        {
            _logger.LogInformation($"Route not found: {ex.Message}");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting route");
            return BadRequest(ex.Message);
        }
    }
}
