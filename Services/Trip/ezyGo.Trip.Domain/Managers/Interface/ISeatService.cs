using ezyGo.Trip.Domain.Models;

namespace ezyGo.Trip.Domain.Managers.Interface;

public interface ISeatService
{
    Task CreateSeatsForTrip(int tripId, string TotalSeat, int Fare);
    Task DeleteSeatsForTrip(int tripId);
    Task<List<SeatModel>> GetAllSeatByTripId(int tripId);
    Task UpdateSeatAvailability(int seatId, bool isSeatBooked);
}
