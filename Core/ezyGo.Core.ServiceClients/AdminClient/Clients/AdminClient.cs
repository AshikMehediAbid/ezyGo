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
    public async Task<List<TripTemplateClientModel>> GetTripTemplateByCompanyId(int companyId)
    {
        var requestUrl = $"api/admin/trip-templates/by-company-id/{companyId}";

        var response = _httpClient.GetAsync(requestUrl);

        if (!await VerifyApiResponse(response)) return new List<TripTemplateClientModel>();

        var tripTemplates = await response.Result.Content.ReadFromJsonAsync<List<TripTemplateClientModel>>();
        return tripTemplates ?? new List<TripTemplateClientModel>();
    }

    private async Task<bool> VerifyApiResponse(Task<HttpResponseMessage> response)
    {
        if (!response.Result.IsSuccessStatusCode)
        {
            var errorContent = await response.Result.Content.ReadAsStringAsync();
            // Log the error content or handle it as needed
            return false;
        }
        return true;
    }
}
