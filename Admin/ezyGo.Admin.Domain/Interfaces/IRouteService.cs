using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Repositories;

namespace ezyGo.Admin.Domain.Interfaces;

public interface IRouteService
{
    Task<Route> CreateRouteAsync(Route route);
    Task DeleteRouteAsync(int id);
    Task<Route> GetRouteByIdAsync(int id);
    Task<IEnumerable<Route>> GetRoutesAsync(string? filter);
    Task UpdateRouteAsync(Route route);
}
