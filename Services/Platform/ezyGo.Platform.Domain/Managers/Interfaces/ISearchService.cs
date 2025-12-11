using ezyGo.Core.ServiceClients.TripClient.Models;

namespace ezyGo.Platform.Domain.Managers.Interfaces;

public interface ISearchService
{
    Task<List<SearchedTripClientResponse>> SearchTripsAsync(string fromCity, string toCity, DateOnly date);
}
