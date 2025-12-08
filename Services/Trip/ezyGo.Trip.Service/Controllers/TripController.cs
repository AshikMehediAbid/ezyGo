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
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
