using ezyGo.Admin.Domain.Models;

namespace ezyGo.Admin.Domain.Interfaces;

public interface IBusService
{
    Task Create(Station bus);
    Task Delete(int id);
    Task<Station> GetById(int id);
    Task<List<Station>> GetStations(string? filter);
    Task Update(Station bus);
}
