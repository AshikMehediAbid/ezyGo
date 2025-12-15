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

    public async Task<List<SeatClientResponse>> GetAllSeatByTripId(int tripId)
    {
        var response = await _httpClient.GetAsync($"api/seat/get-all-by-trip-id?tripId={tripId}");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Request failed with status code {response.StatusCode}"
            );
        }

        var seats = await response.Content.ReadFromJsonAsync<List<SeatClientResponse>>();

        return seats ?? new List<SeatClientResponse>();
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
