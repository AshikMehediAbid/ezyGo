using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using ezyGo.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ezyGo.Admin.Service.Controllers;

[Route("api/admin")]
[ApiController]
public class BusController : ControllerBase
{
    private readonly IBusService _busService;
    private readonly ILogger<BusController> _logger;

    public BusController(IBusService busCompanyService, ILogger<BusController> logger)
    {
        _busService = busCompanyService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a list of all bus companies.
    /// </summary>
    /// <remarks>This method is an HTTP GET endpoint accessible at the route "companies".</remarks>
    /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="IEnumerable{T}"/> of <see cref="BusCompany"/> objects.
    /// Returns an HTTP 200 OK response with the list of bus companies if successful.</returns>
    [HttpGet]
    [Route("companies")]
    public async Task<ActionResult<IEnumerable<BusCompany>>> GetBusCompanies(string? filter)
    {
        var companies = await _busService.GetAllBusCompaniesAsync(filter);
        return Ok(companies);
    }


    /// <summary>
    /// Retrieves a bus company by its unique identifier.
    /// </summary>
    /// <remarks>This method uses the HTTP GET verb and is accessible at the route "company/{id}".</remarks>
    /// <param name="id">The unique identifier of the bus company to retrieve.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing the <see cref="BusCompany"/> if found.  Returns a 404 status code if
    /// the bus company is not found, or a 400 status code if an error occurs.</returns>
    [HttpGet("company/{id}")]
    public async Task<ActionResult<BusCompany>> GetBusCompanyById(int id)
    {
        try
        {
            var company = await _busService.GetBusCompanyByIdAsync(id);
            return Ok(company);
        }
        catch (NotFoundException ex)
        {
            _logger.LogInformation($"Message: {ex.Message}" +
               $"Code: {ex.HttpStatusCode}");

            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Something went wrong");
            return BadRequest($"Something went wrong: {ex.Message}");
        }
    }



    /// <summary>
    /// Creates a new bus company and adds it to the system.
    /// </summary>
    /// <remarks>This method is an HTTP POST endpoint that accepts a <see cref="BusCompany"/> object in the
    /// request body. Ensure that the provided <paramref name="company"/> object contains valid data before calling this
    /// method.</remarks>
    /// <param name="company">The <see cref="BusCompany"/> object containing the details of the bus company to create.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing the created <see cref="BusCompany"/> object if the operation is
    /// successful. Returns a <see cref="BadRequestObjectResult"/> with an error message if the operation fails.</returns>
    [HttpPost]
    [Route("company/create")]
    public async Task<ActionResult<BusCompany>> CreateBusCompany(BusCompany company)
    {
        try
        {
            var createdCompany = await _busService.CreateBusCompanyAsync(company);
            return Ok(company);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }




    /// <summary>
    /// Updates the details of an existing bus company.
    /// </summary>
    /// <param name="id">The unique identifier of the bus company to update.</param>
    /// <param name="company">The updated <see cref="BusCompany"/> object containing the new details.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Returns <see cref="NoContentResult"/> if
    /// the update is successful, or <see cref="NotFoundResult"/>  if the specified bus company is not found.</returns>
    [HttpPut]
    [Route("company/update/{id}")]
    public async Task<IActionResult> UpdateBusCompany(int id, BusCompany company)
    {
        try
        {
            await _busService.UpdateBusCompanyAsync(id, company);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }





    /// <summary>
    /// Deletes a bus company with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the bus company to delete.</param>
    /// <returns>A <see cref="NoContentResult"/> if the deletion is successful;  otherwise, a <see cref="NotFoundResult"/> if the
    /// specified bus company does not exist.</returns>
    [HttpDelete]
    [Route("company/delete/{id}")]
    public async Task<IActionResult> DeleteBusCompany(int id)
    {
        try
        {
            await _busService.DeleteBusCompanyAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }





    /// <summary>
    /// Retrieves a collection of all available buses.
    /// </summary>
    /// <remarks>This method sends an HTTP GET request to the "buses" route and returns the result as an HTTP
    /// 200 OK response  containing the list of buses. If no buses are available, the response will contain an empty
    /// collection.</remarks>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all buses. The collection will be empty if no buses are available.</returns>
    [HttpGet]
    [Route("buses")]
    public async Task<ActionResult<IEnumerable<Bus>>> GetBuses(string? filter)
    {
        var buses = await _busService.GetAllBusesAsync(filter);
        return Ok(buses);
    }





    /// <summary>
    /// Retrieves a bus by its unique identifier.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to fetch the bus details from the data
    /// source.</remarks>
    /// <param name="id">The unique identifier of the bus to retrieve.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing the bus with the specified identifier if found;  otherwise, a <see
    /// cref="NotFoundResult"/> if no bus with the given identifier exists.</returns>
    [HttpGet("bus/{id}")]
    public async Task<ActionResult<Bus>> GetBusById(int id)
    {
        var company = await _busService.GetBusByIdAsync(id);
        if (company == null)
            return NotFound();

        return Ok(company);
    }




    [HttpPost]
    [Route("bus/create")]
    public async Task<ActionResult<BusCompany>> CreateBus([FromForm]Bus bus)
    {
        try
        {
            var createdBus = await _busService.CreateBusAsync(bus);
            return Ok(bus);
        }
        catch (NotFoundException ex)
        {
            _logger.LogInformation(ex, $"Not Found: {ex.Message}");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Something went wrong");
            return BadRequest(ex.Message);
        }
    }


    [HttpPut]
    [Route("bus/update/{id}")]
    public async Task<IActionResult> UpdateBus(int id, Bus bus)
    {
        try
        {
            await _busService.UpdateBusAsync(id, bus);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    [Route("bus/delete/{id}")]
    public async Task<IActionResult> DeleteBus(int id)
    {
        try
        {
            await _busService.DeleteBusAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
