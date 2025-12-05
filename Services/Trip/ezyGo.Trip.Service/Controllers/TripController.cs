using ezyGo.Trip.Domain.Managers.Interface;
using Microsoft.AspNetCore.Http;
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
    public IActionResult GetTemplateTripByCompanyId(int id)
    {
        var templateTrips = _service.GetTemplateTripByCompanyId(id);

        return Ok(new { CompanyId = id, TemplateTrip = "Sample Template Trip Data" });
    }
}
