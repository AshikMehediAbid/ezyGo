using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ezyGo.Admin.Service.Controllers;

[Route("api/trip-template")]
[ApiController]
public class TripTemplateController : ControllerBase
{
    private readonly ITripTemplateService _service;
    private readonly ILogger<TripTemplateController> _logger;

    public TripTemplateController(ITripTemplateService Service, ILogger<TripTemplateController> logger)
    {
        _service = Service;
        _logger = logger;
    }


    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreatTripTemplate(TripTemplateModel tripTemplate)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            TripTemplateModel trip = await _service.CreateTripTemplateAsync(tripTemplate);
            return Ok(trip);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
}
