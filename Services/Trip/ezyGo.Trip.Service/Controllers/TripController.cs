using ezyGo.Core.Exceptions;
using ezyGo.Trip.Domain.Managers.Interface;
using ezyGo.Trip.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ezyGo.Trip.Service.Controllers;

[Route("api/trip")]
[ApiController]
public class TripController : ControllerBase
{
    private readonly ITripService _service;
    private readonly ILogger<TripController> _logger;

    public TripController(ITripService service, ILogger<TripController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost]
    [Route("search")]
    public async Task<IActionResult> GetAllTripByUserSearchRequest([FromBody] UserTripRequest tripRequest)
    {
        try
        {
            var trips = await _service.GetAllTripByUserSearchRequest(tripRequest);
            return Ok(trips);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var trip = await _service.GetTripById(id);
        if (trip is null)
        {
            return NotFound();
        }
        return Ok(trip);
    }


    [HttpGet]
    [Route("template-trip-by-company/{id}")]
    public async Task<IActionResult> GetTemplateTripByCompanyId(int id)
    {
        try
        {
            var templateTrips = await _service.GetTripTemplateByCompanyId(id);

            return Ok(templateTrips);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPost]
    [Route("sehedule-trip")]
    public async Task<IActionResult> ScheduleTrip(TripDetailsModel tripDetails)
    {
        try
        {
            TripDetailsModel trip = await _service.ScheduleTrip(tripDetails);
            return Ok(trip);
        }
        catch (AlreadyExistException ex)
        {
            _logger.LogError(ex, "Skip scheduling the Trip. Trip already exists.");
            return Ok(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update(int id, TripDetailsModel trip)
    {
        try
        {
            var updated = await _service.UpdateTrip(id, trip);
            return Ok(updated);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update trip {TripId}", id);
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _service.DeleteTrip(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete trip {TripId}", id);
            return BadRequest(ex.Message);
        }
    }
}
