using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ezyGo.Admin.Service.Controllers;

[Route("api/admin")]
[ApiController]
public class BusController : ControllerBase
{
    private readonly IBusService _busService;

    public BusController(IBusService busService)
    {
        _busService = busService;
    }

    [HttpPost]
    [Route("add-bus-station")]
    public async Task<IActionResult> CreateStation([FromForm] Station station)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _busService.Create(station);
            return Ok(station);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



    [HttpPut]
    [Route("update-bus-station")]
    public async Task<IActionResult> UpdateStation([FromForm] Station station)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _busService.Update(station);
            return Ok(station);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
