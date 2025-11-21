using ezyGo.Core.ServiceClients.VehicleClient.Models;

namespace ezyGo.Core.ServiceClients.VehicleClient.Clients;

public interface IVehicleClient
{
    Task<List<VehicleSearchRequestClientModel>> SearchVehiclesAsync(VehicleSearchRequestClientModel request);
}
