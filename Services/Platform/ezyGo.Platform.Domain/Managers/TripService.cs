using ezyGo.Core.ServiceClients.AdminClient.Clients;
using ezyGo.Core.ServiceClients.TripClient.Clients;
using ezyGo.Core.ServiceClients.TripClient.Models;
using ezyGo.Platform.Domain.Managers.Interfaces;

namespace ezyGo.Platform.Domain.Managers;

public class TripService : ITripService
{
    private readonly ITripClient _tripClient;
    private readonly IAdminClient _adminClient;

    public TripService(ITripClient tripClient, IAdminClient adminClient)
    {
        _tripClient = tripClient;
        _adminClient = adminClient;
    }

    public async Task<List<SeatClientResponse>> GetAllSeatByTripId(int tripId)
    {
        var seats = await _tripClient.GetAllSeatByTripId(tripId);
        return seats;
    }

    public async Task<List<string>> SearchLocationAsync(string filter)
    {
        var locations = await _adminClient.GetAllCounter(filter);
        return locations;
    }

    public async Task<List<SearchedTripClientResponse>> SearchTripsAsync(string fromCity, string toCity, DateOnly date)
    {
        var trips = await _tripClient.SearchTripsAsync(fromCity, toCity, date);
        return trips;
    }
}
