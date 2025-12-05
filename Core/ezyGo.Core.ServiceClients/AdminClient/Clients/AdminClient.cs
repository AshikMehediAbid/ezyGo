using ezyGo.Core.ServiceClients.AdminClient.Models;
using System.Net.Http.Json;

namespace ezyGo.Core.ServiceClients.AdminClient.Clients;

public class AdminClient : IAdminClient
{
    private readonly HttpClient _httpClient;

    public AdminClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<TripTemplateClientResponse>> GetTripTemplateByCompanyId(int companyId)
    {
        var requestUrl = $"api/trip-template/by-company/{companyId}";

        var response = await _httpClient.GetAsync(requestUrl);

        if (!await VerifyApiResponse(response)) return new List<TripTemplateClientResponse>();

        var tripTemplates = await response.Content.ReadFromJsonAsync<List<TripTemplateClientResponse>>();
        return tripTemplates ?? new List<TripTemplateClientResponse>();
    }

    private async Task<bool> VerifyApiResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            // Log the error content or handle it as needed
            return false;
        }
        return true;
    }
}
