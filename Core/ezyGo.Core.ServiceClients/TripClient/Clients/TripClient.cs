using ezyGo.Core.ServiceClients.TripClient.Models;
using System.Net.Http.Json;

namespace ezyGo.Core.ServiceClients.TripClient.Clients;

public class TripClient : ITripClient
{
    private readonly HttpClient _httpClient;
    public TripClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<SearchedTripClientResponse>> SearchTripsAsync(string fromCity, string toCity, DateOnly date)
    {
        var request = new TripClientRequest
        {
            FromLocation = fromCity,
            ToLocation = toCity,
            TripDate = date
        };

        var response = await _httpClient.PostAsJsonAsync("api/trip/search", request);

        if(!response.IsSuccessStatusCode)
        {
            // Handle error response as needed
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
        }

        var trips = await response.Content.ReadFromJsonAsync<List<SearchedTripClientResponse>>();

        return trips ?? new List<SearchedTripClientResponse>();

    }
}
