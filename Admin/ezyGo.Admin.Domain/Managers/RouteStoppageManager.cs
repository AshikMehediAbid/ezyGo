using AutoMapper;
using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Repositories;
using ezyGo.Core.Exceptions;

namespace ezyGo.Admin.Domain.Managers;

public class RouteStoppageManager : IRouteStoppageManager
{
    private readonly IRouteStoppageRepository _repo;
    private readonly IMapper _mapper;

    public RouteStoppageManager(IRouteStoppageRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }
    public async Task<RouteStoppage> AddStoppageEndAsync(RouteStoppage model)
    {
        bool isExist = await _repo.IsStoppageAlreadyExist(model.RouteId, model.StationId);

        if (isExist)
            throw new AlreadyExistException($"The Stoppage: \"{model.StationId}\" is already assigned in the route \"{model.RouteId}\"");


        var stoppageEntity = _mapper.Map<RouteStoppageEntity>(model);
        var stoppage = await _repo.InsertAtEndAsync(stoppageEntity);

        return _mapper.Map<RouteStoppage>(stoppage);
    }

    public async Task<RouteStoppage> AddStoppageMiddleAsync(int routeId, int stationId, int insertAfterOrder)
    {
        bool isExist = await _repo.IsStoppageAlreadyExist(routeId, stationId);

        if (isExist)
            throw new AlreadyExistException($"The Stoppage: \"{stationId}\" is already assigned in the route \"{routeId}\"");

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
