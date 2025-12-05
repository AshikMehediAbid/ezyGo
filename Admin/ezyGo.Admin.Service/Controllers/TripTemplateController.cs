using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using ezyGo.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ezyGo.Admin.Service.Controllers;

[Route("api/trip-template")]
[ApiController]
public class TripTemplateController : ControllerBase
{
    private readonly ITripTemplateService _service;
    private readonly ILogger<TripTemplateController> _logger;

    public TripTemplateController(ITripTemplateService service, ILogger<TripTemplateController> logger)
    {
        _service = service;
        _logger = logger;
    }


    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetTripTemplates([FromQuery] string? filter)
    {
        var templates = await _service.GetTripTemplatesAsync(filter);
        return Ok(templates);
    }



    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetTripTemplateById(int id)
    {
        try
        {
            var template = await _service.GetTripTemplateByIdAsync(id);
            return Ok(template);
        }
        catch (NotFoundException ex)
        {
            _logger.LogInformation(ex, "Trip template {TripTemplateId} not found.", id);
            return NotFound(ex.Message);
        }
    }


    [HttpGet]
    [Route("by-company/{companyId}")]
    public async Task<IActionResult> GetTripTemplateByCompanyId(int companyId)
    {
        var templates = await _service.GetTripTemplatesByCompanyIdAsync(companyId);
        return Ok(templates);

    }



    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreateTripTemplate([FromBody] TripTemplateModel tripTemplate)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            TripTemplateModel trip = await _service.CreateTripTemplateAsync(tripTemplate);
            return Ok(trip);
        }
        catch (NotFoundException ex)
        {
            _logger.LogInformation(ex, "Dependency not found while creating trip template.");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create trip template.");
            return BadRequest(ex.Message);
        }
    }



    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateTripTemplate(int id, [FromBody] TripTemplateModel tripTemplate)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _service.UpdateTripTemplateAsync(id, tripTemplate);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            _logger.LogInformation(ex, "Trip template {TripTemplateId} not found for update.", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update trip template {TripTemplateId}.", id);
            return BadRequest(ex.Message);
        }
    }



    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteTripTemplate(int id)
    {
        try
        {
            await _service.DeleteTripTemplateAsync(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            _logger.LogInformation(ex, "Trip template {TripTemplateId} not found for delete.", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete trip template {TripTemplateId}.", id);
            return BadRequest(ex.Message);
        }
    }
}
