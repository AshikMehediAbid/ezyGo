using ezyGo.Core.ServiceClients.TripClient.Models;

namespace ezyGo.Core.ServiceClients.TripClient.Clients;

public interface ITripClient
{
    Task<List<SeatClientResponse>> GetAllSeatByTripId(int tripId);
    Task<List<SearchedTripClientResponse>> SearchTripsAsync(string fromCity, string toCity, DateOnly date);
}
