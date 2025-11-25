using ezyGo.Admin.Domain.Models;

namespace ezyGo.Admin.Domain.Interfaces;

public interface IBusStationRepository
{
    Task Create(Station busStation);
    Task Delete(int id);
    Task<Station> GetById(int id);
    Task<List<Station>> GetStations(string? filter);
    Task Update(Station busStation);
}
