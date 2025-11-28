using AutoMapper;
using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Repositories;

namespace ezyGo.Admin.Domain.Managers;

public class RouteStoppageManager : IRouteStoppageManager
{
    private readonly IRouteStoppageRepository _repo;
    private readonly IMapper _mapper;

    public RouteStoppageManager(IRouteStoppageRepository repo)
    {
        _repo = repo;
    }
    public async Task<RouteStoppage> AddStoppageEndAsync(RouteStoppage model)
    {
        var stoppageEntity = _mapper.Map<RouteStoppageEntity>(model);
        var stoppage = await _repo.InsertAtEndAsync(stoppageEntity);

        return _mapper.Map<RouteStoppage>(stoppageEntity);
    }

    public async Task<RouteStoppage> AddStoppageMiddleAsync(int routeId, int stationId, int insertAfterOrder)
    {
        var stoppage = await _repo.InsertInMiddleAsync(routeId, stationId, insertAfterOrder);

        return _mapper.Map<RouteStoppage>(stoppage);
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<RouteStoppage>> GetStoppagesAsync(int routeId)
    {
        var stoppages = await _repo.GetByRouteIdAsync(routeId);

        return _mapper.Map<List<RouteStoppage>>(stoppages);
    }

    public Task<bool> ReorderAsync(int routeId, List<int> stationIds)
    {
        throw new NotImplementedException();
    }
}
