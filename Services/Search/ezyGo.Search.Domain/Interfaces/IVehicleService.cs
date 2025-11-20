using ezyGo.Search.Domain.Models;

namespace ezyGo.Search.Domain.Interfaces;

public interface IVehicleService
{
    Task<List<VehicleSearchRequest>> SearchVehiclesAsync(VehicleSearchRequest request);
}
