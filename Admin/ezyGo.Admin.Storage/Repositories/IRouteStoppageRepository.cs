using ezyGo.Admin.Storage.Entities;
using ezyGo.EntityFrameworkCore.Repository;

namespace ezyGo.Admin.Storage.Repositories;

public interface IRouteStoppageRepository : IGenericRepository<RouteStoppageEntity>
{
    Task<IEnumerable<RouteStoppageEntity>> GetByRouteIdAsync(int routeId);
    Task<RouteStoppageEntity> InsertAtEndAsync(RouteStoppageEntity model);
    Task<RouteStoppageEntity> InsertInMiddleAsync(int routeId, int stationId, int insertAfterOrder);
    Task<bool> UpdateOrderAsync(int id, int newOrder);
    Task<bool> DeleteAsync(int id);
    Task<bool> IsStoppageAlreadyExist(int routeId, int stationId);
}
