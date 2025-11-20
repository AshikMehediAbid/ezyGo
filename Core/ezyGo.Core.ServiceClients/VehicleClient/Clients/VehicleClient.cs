using ezyGo.Core.ServiceClients.VehicleClient.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace ezyGo.Core.ServiceClients.VehicleClient.Clients;

public class VehicleClient : IVehicleClient
{
    private readonly HttpClient _httpClient;

    public VehicleClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<VehicleSearchRequestClientModel>> SearchVehiclesAsync(VehicleSearchRequestClientModel request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/utility/save-application", request);
        throw new NotImplementedException();
    }
}
