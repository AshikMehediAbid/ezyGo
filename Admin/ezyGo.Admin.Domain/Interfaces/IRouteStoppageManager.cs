using ezyGo.Admin.Domain.Models;

namespace ezyGo.Admin.Domain.Interfaces;

public interface IRouteStoppageManager
{
    Task<RouteStoppage> AddStoppageEndAsync(RouteStoppage model);
    Task<RouteStoppage> AddStoppageMiddleAsync(int routeId, int stationId, int insertAfterOrder);
    Task<IEnumerable<RouteStoppage>> GetStoppagesAsync(int routeId);
    Task<bool> ReorderAsync(int routeId, List<int> stationIds);
    Task<bool> DeleteAsync(int id);
}
