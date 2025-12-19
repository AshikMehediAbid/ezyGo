using ezyGo.Admin.Domain.Models;

namespace ezyGo.Admin.Domain.Interfaces;

public interface IBusStationService
{
    Task CreateBusStationAsync(Station bus);
    Task DeleteBusStationAsync(int id);
    Task<Station> GetBusStationByIdAsync(int id);
    Task<IEnumerable<Station>> GetBusStationsAsync(string? filter);
    Task<IEnumerable<string>> GetStationsNameAsync(string? filter);
    Task UpdateBusStationAsync(Station bus);
}
