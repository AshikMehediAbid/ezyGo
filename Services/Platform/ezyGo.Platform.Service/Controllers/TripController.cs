using ezyGo.Platform.Domain.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ezyGo.Platform.Service.Controllers;

[Route("api/trip")]
[ApiController]
public class TripController : ControllerBase
{
    private readonly ITripService _tripService;
    private readonly ILogger<TripController> _logger;

    public TripController(ITripService tripService, ILogger<TripController> logger)
    {
        _tripService = tripService;
        _logger = logger;
    }

    [HttpGet]
    [Route("search")]
    public async Task<IActionResult> Search([FromQuery] string fromCity, [FromQuery] string toCity, [FromQuery] DateOnly date)
    {
        try
        {

            var trips = await _tripService.SearchTripsAsync(fromCity, toCity, date);
            _logger.LogInformation("Searched for trips from {FromCity} to {ToCity} on {Date}", fromCity, toCity, date);
            return Ok(trips);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,$"\n{ex.InnerException.Message}\n", "Error occurred while searching for trips from {FromCity} to {ToCity} on {Date}", fromCity, toCity, date);
            return StatusCode(500, "An error occurred while processing your request.");
        }

    }

    [HttpGet]
    [Route("seats/{tripId}")]
    public async Task<IActionResult> GetAllSeatByTripId(int tripId)
    {
        try
        {

            var seats = await _tripService.GetAllSeatByTripId(tripId);

            return Ok(seats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
