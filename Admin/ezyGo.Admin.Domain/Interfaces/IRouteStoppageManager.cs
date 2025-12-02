using ezyGo.Admin.Domain.Models;

namespace ezyGo.Admin.Domain.Interfaces;

public interface IRouteStoppageManager
{
    Task<RouteStoppage> AddStoppageEndAsync(RouteStoppage model);
    Task<RouteStoppage> AddStoppageMiddleAsync(int routeId, int stationId, int insertAfterOrder);
    Task<RouteStoppage> GetByIdAsync(int id);
    Task<IEnumerable<RouteStoppage>> GetStoppagesAsync(int routeId);
    Task UpdateOrderAsync(int id, int newOrder);
    Task UpdateAsync(int id, RouteStoppage model);
    Task<bool> DeleteAsync(int id);
}
