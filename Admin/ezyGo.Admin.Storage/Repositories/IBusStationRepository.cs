using ezyGo.Admin.Storage.Entities;
using ezyGo.EntityFrameworkCore.Repository;

namespace ezyGo.Admin.Storage.Repositories;

public interface IBusStationRepository : IGenericRepository<BusStationEntity>
{
    Task<List<BusStationEntity>> GetStations(string? filter);
    Task DeleteBusStationWithDependenciesAsync(BusStationEntity station);
}
