using AutoMapper;
using ezyGo.Trip.Domain.Managers.Interface;
using ezyGo.Trip.Domain.Models;
using ezyGo.Trip.Storage.Entities;
using ezyGo.Trip.Storage.Repositories.Interfaces;

namespace ezyGo.Trip.Domain.Managers;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _seatRepo;
    private readonly IMapper _mapper;
    public SeatService(ISeatRepository seatRepo, IMapper mapper)
    {
        _seatRepo = seatRepo;
        _mapper = mapper;
    }

    public async Task CreateSeatsForTrip(int tripId, string totalSeat, int fare)
    {

        var (seatRow, seatColumn, isExtraSeat) = GetRowColumn(int.Parse(totalSeat));

        for(int i = 1; i<= seatColumn; i++)
        {
            for(int j = 1; j<=seatRow; j++)
            {
                var seatEntity = new Seat
                {
                    TripId = tripId,
                    SeatNumber = $"{(char)(64 + j)}{i}",
                    IsAvailable = true,
                    SeatFare = fare,
                    SeatRow = j,
                    SeatColumn = i
                };
                await _seatRepo.AddAsync(seatEntity);
            }
        }
        if(isExtraSeat)
        {
            var seatEntity = new Seat
            {
                TripId = tripId,
                SeatNumber = $"{(char)(64 + seatRow  )}{seatColumn+1}",
                IsAvailable = true,
                SeatFare = fare,
                SeatRow = seatRow,
                SeatColumn = seatColumn+1
            };
            await _seatRepo.AddAsync(seatEntity);
        }
    }

    public async Task DeleteSeatsForTrip(int tripId)
    {
        await _seatRepo.DeleteSeatByTripId(tripId);
    }

    public async Task<List<SeatModel>> GetAllSeatByTripId(int tripId)
    {
        var seats = await _seatRepo.GetAllSeatByTripId(tripId);

        return _mapper.Map<List<SeatModel>>(seats);
    }

    public async Task UpdateSeatAvailability(int seatId, bool isSeatBooked)
    {
        var seat = await _seatRepo.GetByIdAsync(seatId);
        if (seat == null)
        {
            throw new KeyNotFoundException($"Seat with ID {seatId} not found.");
        }
        seat.IsAvailable = !isSeatBooked;

        await _seatRepo.UpdateAsync(seat);
    }

    private (int seatRow, int seatColumn, bool isExtra) GetRowColumn(int totalSeat)
    {
        int seatRow = 0;
        int seatColumn = 0;
        bool isExtraSeat = false;
        

        if(totalSeat<=31)
        {
            seatColumn = 3;
            seatRow = totalSeat / 3;

            isExtraSeat = (totalSeat % 3) != 0;
        }
        else
        {
            seatColumn = 4;
            seatRow = totalSeat / 4;
            isExtraSeat = (totalSeat % 4) != 0;
        }

        return (seatRow, seatColumn, isExtraSeat);
    }
}
