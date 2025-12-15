using ezyGo.Platform.Domain.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ezyGo.Platform.Service.Controllers;

[Route("api/booking")]
[ApiController]
public class SeatController : ControllerBase
{
    private readonly ISeatService _seatService;
    public SeatController(ISeatService seatService)
    {
        _seatService = seatService;
    }

    [HttpPost]
    [Route("reserve-seat")]
    public async Task<IActionResult> ReserveSeat([FromQuery] int seatNo)
    {
        try
        {
            var isAvailable =  await _seatService.ReserveSeatAsync(seatNo);

            if (isAvailable)
                return Ok($"Seat NO: {seatNo} is reserved");
            else
                return NotFound($"Seat NO: {seatNo} is not available currently!");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }


    [HttpPost]
    [Route("release-seat")]
    public async Task<IActionResult> ReleaseSeat([FromQuery] int seatNo)
    {
        try
        {
            var isDeleted = await _seatService.ReleaseSeatAsync(seatNo);

            if (isDeleted)
                return Ok($"Seat NO: {seatNo} is successfully released!");
            else
                return NotFound($"Something went wrong");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
