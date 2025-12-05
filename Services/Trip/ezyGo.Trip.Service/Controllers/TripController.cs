using ezyGo.Trip.Domain.Managers.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
}
