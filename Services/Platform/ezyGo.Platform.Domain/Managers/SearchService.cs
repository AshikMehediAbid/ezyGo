using ezyGo.Core.ServiceClients.TripClient.Clients;
using ezyGo.Core.ServiceClients.TripClient.Models;
using ezyGo.Platform.Domain.Managers.Interfaces;

namespace ezyGo.Platform.Domain.Managers;

public class SearchService : ISearchService
{
    private readonly ITripClient _tripClient;

    public SearchService(ITripClient tripClient)
    {
        _tripClient = tripClient;
    }
    public async Task<List<SearchedTripClientResponse>> SearchTripsAsync(string fromCity, string toCity, DateOnly date)
    {
        var trips = await _tripClient.SearchTripsAsync(fromCity, toCity, date);
        return trips;
    }
}
