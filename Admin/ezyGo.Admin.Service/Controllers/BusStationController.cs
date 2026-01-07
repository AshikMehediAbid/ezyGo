using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using ezyGo.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ezyGo.Admin.Service.Controllers;

[Route("api/admin")]
[ApiController]
public class BusStationController : ControllerBase
{
    private readonly IBusStationService _busStationService;
    private readonly ILogger<BusStationController> _logger;

    public BusStationController(IBusStationService busService, ILogger<BusStationController> logger)
    {
        _busStationService = busService;
        _logger = logger;
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
            await _busStationService.CreateBusStationAsync(station);
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
            await _busStationService.UpdateBusStationAsync(station);
            return Ok(station);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }




    [HttpGet]
    [Route("bus-station/{id}")]
    public async Task<IActionResult> GetStationById(int id)
    {
        try
        {
            Station station = await _busStationService.GetBusStationByIdAsync(id);
            return Ok(station);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message, "Something went wrong while fetching the station.");
            return BadRequest(ex.Message);
        }

    }


    [HttpGet]
    [Route("bus-station")]
    public async Task<IActionResult> GetStations(string? filter)
    {
        try
        {
            var stations =await _busStationService.GetBusStationsAsync(filter);
            return Ok(stations);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message.ToString());

        }
        
    }

    [HttpGet]
    [Route("station-name")]
    public async Task<IActionResult> GetAllStationsName(string? filter)
    {
        try
        {
            var stations = await _busStationService.GetStationsNameAsync(filter);
            return Ok(stations);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message.ToString());

        }

    }

    [HttpDelete]
    [Route("bus-station")]
    public async Task<IActionResult> DeleteStations(int id)
    {
        try
        {
            await _busStationService.DeleteBusStationAsync(id);
            return Ok($"Station with id: {id} is deleted!");
        }
        catch(NotFoundException ex)
        {
            _logger.LogInformation($"Message: {ex.Message}" +
                $"Code: {ex.HttpStatusCode}");

            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message.ToString());

        }

    }
}
