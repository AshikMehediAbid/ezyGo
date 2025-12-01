using ezyGo.Admin.Storage.Entities;
using ezyGo.EntityFrameworkCore.Repository;

namespace ezyGo.Admin.Storage.Repositories;

public interface IRouteRepository : IGenericRepository<RouteEntity>
{
    Task<RouteEntity> CreateRouteAsync(RouteEntity route);
    Task<List<RouteEntity>> GetRoutesAsync(string? filter);
    Task<bool> IsRouteExist(int startId, int endId);
    Task<RouteEntity?> GetRouteByIdWithDetailsAsync(int id);
}
