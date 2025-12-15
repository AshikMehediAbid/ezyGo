using ezyGo.Core.ServiceClients.TripClient.Models;

namespace ezyGo.Platform.Domain.Managers.Interfaces;

public interface ITripService
{
    Task<List<SeatClientResponse>> GetAllSeatByTripId(int tripId);
    Task<List<SearchedTripClientResponse>> SearchTripsAsync(string fromCity, string toCity, DateOnly date);
}
