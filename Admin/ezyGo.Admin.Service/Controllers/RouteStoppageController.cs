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
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("add-end")]
    public async Task<IActionResult> AddAtEnd(RouteStoppage model)
    {
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

    [HttpPost("reorder")]
    public async Task<IActionResult> Reorder(int routeId, List<int> stationIds)
    {
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _manager.DeleteAsync(id);
        return Ok();
    }

}
