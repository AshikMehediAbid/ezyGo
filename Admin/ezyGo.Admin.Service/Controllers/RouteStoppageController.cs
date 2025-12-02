using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using ezyGo.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ezyGo.Admin.Service.Controllers;

[Route("api/stoppages")]
[ApiController]
public class RouteStoppageController : ControllerBase
{
    private readonly IRouteStoppageManager _manager;
    private readonly ILogger<RouteStoppageController> _logger;

    public RouteStoppageController(IRouteStoppageManager manager, ILogger<RouteStoppageController> logger)
    {
        _manager = manager;
        _logger = logger;
    }

    [HttpGet]
    [Route("all/{routeId}")]
    public async Task<IActionResult> GetStoppagesAsync(int routeId)
    {
        try
        {
            var stoppages = await _manager.GetStoppagesAsync(routeId);
            return Ok(stoppages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get stoppages for route {RouteId}.", routeId);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetStoppagesById(int id)
    {
        try
        {
            var stoppage = await _manager.GetByIdAsync(id);
            return Ok(stoppage);
        }
        catch (NotFoundException ex)
        {
            _logger.LogInformation(ex, "Stoppage {StoppageId} not found.", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get stoppage {StoppageId}.", id);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("add-end")]
    public async Task<IActionResult> AddAtEnd(RouteStoppage model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var stoppages = await _manager.AddStoppageEndAsync(model);
            return Ok(model);
        }
        catch (AlreadyExistException ex)
        {
            return Ok(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("add-middle")]
    public async Task<IActionResult> AddMiddle(int routeId, int stationId, int insertAfterOrder)
    {
        try
        {
            var stoppage = await _manager.AddStoppageMiddleAsync(routeId, stationId, insertAfterOrder);
            return Ok(stoppage);
        }
        catch(AlreadyExistException ex)
        {
            return Ok(ex.Message);
        }
        catch (Exception ex )
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPut]
    [Route("update/{id}")]
    public async Task<IActionResult> UpdateStoppage(int id, [FromBody] RouteStoppage model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _manager.UpdateAsync(id, model);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            _logger.LogInformation(ex, "Stoppage {StoppageId} not found for update.", id);
            return NotFound(ex.Message);
        }
        catch (AlreadyExistException ex)
        {
            _logger.LogInformation(ex, "Stoppage already exists for route.");
            return Ok(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update stoppage {StoppageId}.", id);
            return BadRequest(ex.Message);
        }
    }


    [HttpPut]
    [Route("update/{id}/order")]
    public async Task<IActionResult> UpdateOrder(int id, [FromQuery] int newOrder)
    {
        if (newOrder <= 0)
            return BadRequest("newOrder must be greater than zero.");

        try
        {
            await _manager.UpdateOrderAsync(id, newOrder);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            _logger.LogInformation(ex, "Stoppage {StoppageId} not found for order update.", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update order for stoppage {StoppageId}.", id);
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _manager.DeleteAsync(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            _logger.LogInformation(ex, "Stoppage {StoppageId} not found for delete.", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete stoppage {StoppageId}.", id);
            return BadRequest(ex.Message);
        }
    }

}
