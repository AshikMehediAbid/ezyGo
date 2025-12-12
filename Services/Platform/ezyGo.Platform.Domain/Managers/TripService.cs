using ezyGo.Core.ServiceClients.TripClient.Clients;
using ezyGo.Core.ServiceClients.TripClient.Models;
using ezyGo.Platform.Domain.Managers.Interfaces;

namespace ezyGo.Platform.Domain.Managers;

public class TripService : ITripService
{
    private readonly ITripClient _tripClient;

    public TripService(ITripClient tripClient)
    {
        _tripClient = tripClient;
    }

    public async Task<List<SeatClientResponse>> GetAllSeatByTripId(int tripId)
    {
        var seats = await _tripClient.GetAllSeatByTripId(tripId);
        return seats;
    }

    public async Task<List<SearchedTripClientResponse>> SearchTripsAsync(string fromCity, string toCity, DateOnly date)
    {
        var trips = await _tripClient.SearchTripsAsync(fromCity, toCity, date);
        return trips;
    }
}
