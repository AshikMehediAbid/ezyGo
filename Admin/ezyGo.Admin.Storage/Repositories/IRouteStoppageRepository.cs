using ezyGo.Admin.Storage.Entities;

namespace ezyGo.Admin.Storage.Repositories;

public interface IRouteStoppageRepository
{
    Task<IEnumerable<RouteStoppageEntity>> GetByRouteIdAsync(int routeId);
    Task<RouteStoppageEntity> InsertAtEndAsync(RouteStoppageEntity model);
    Task<RouteStoppageEntity> InsertInMiddleAsync(int routeId, int stationId, int insertAfterOrder);
    Task<bool> ReorderAsync(int routeId, List<int> stationIds);
    Task<bool> DeleteAsync(int id);
    Task<bool> IsStoppageAlreadyExist(int routeId, int stationId);
}
