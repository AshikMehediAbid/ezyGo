using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ezyGo.Admin.Service.Controllers;

[Route("api/admin")]
[ApiController]
public class BusController : ControllerBase
{
    private readonly IBusCompanyService _busCompanyService;

    public BusController(IBusCompanyService busCompanyService)
    {
        _busCompanyService = busCompanyService;
    }

    [HttpGet]
    [Route("companies")]
    public async Task<ActionResult<IEnumerable<BusCompany>>> GetBusCompanies()
    {
        var companies = await _busCompanyService.GetAllBusCompaniesAsync();
        return Ok(companies);
    }

    [HttpGet("company/{id}")]
    public async Task<ActionResult<BusCompany>> GetBusCompanyById(int id)
    {
        var company = await _busCompanyService.GetBusCompanyByIdAsync(id);
        if (company == null)
            return NotFound();

        return Ok(company);
    }


    [HttpPost]
    [Route("company/create")]
    public async Task<ActionResult<BusCompany>> CreateBusCompany(BusCompany company)
    {
        try
        {
            var createdCompany = await _busCompanyService.CreateBusCompanyAsync(company);
            return Ok(company);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPut]
    [Route("company/update/{id}")]
    public async Task<IActionResult> UpdateBusCompany(int id, BusCompany company)
    {
        try
        {
            await _busCompanyService.UpdateBusCompanyAsync(id, company);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    [Route("company/delete/{id}")]
    public async Task<IActionResult> DeleteBusCompany(int id)
    {
        try
        {
            await _busCompanyService.DeleteBusCompanyAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
