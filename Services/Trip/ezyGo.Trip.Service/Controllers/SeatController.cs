using ezyGo.Trip.Domain.Managers.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ezyGo.Trip.Service.Controllers;

[Route("api/seat")]
[ApiController]
public class SeatController : ControllerBase
{
    private readonly ISeatService _seatService;

    public SeatController(ISeatService seatService)
    {
        _seatService = seatService;
    }


    [HttpPost]
    [Route("create-all-for-a-trip")]
    public async Task<IActionResult> CreateSeats(int tripId, string totalSeat, int fare)
    {
        try
        {
            await _seatService.CreateSeatsForTrip(tripId, totalSeat, fare);

            return Ok($" {totalSeat} Seats created successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpGet]
    [Route("get-all-by-trip-id")]
    public async Task<IActionResult> GetAllSeatByTripId(int tripId)
    {
        try
        {
            var seats = await _seatService.GetAllSeatByTripId(tripId);
            return Ok(seats);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("update-seat")]
    public async Task<IActionResult> ConfirmSeat([FromBody] string seats)
    {
        try
        {
            await _seatService.ConfirmSeat(seats);

            return Ok("Seat status Updated");

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);

        }
    }

    [HttpDelete]
    [Route("delete-all/{tripId}")]
    public async Task<IActionResult> DeleteAllSeatByTripId(int tripId)
    {
        try
        {
            await _seatService.DeleteSeatsForTrip(tripId);
            return Ok($"All Seat deleted for trip: {tripId}");
        }
        catch(Exception ex)
        {
            return BadRequest("Seat delation failed");
        }
    }
}
