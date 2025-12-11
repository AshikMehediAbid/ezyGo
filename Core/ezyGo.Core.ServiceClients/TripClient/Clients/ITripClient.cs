using ezyGo.Core.ServiceClients.TripClient.Models;

namespace ezyGo.Core.ServiceClients.TripClient.Clients;

public interface ITripClient
{
    Task<List<SearchedTripClientResponse>> SearchTripsAsync(string fromCity, string toCity, DateOnly date);
}
