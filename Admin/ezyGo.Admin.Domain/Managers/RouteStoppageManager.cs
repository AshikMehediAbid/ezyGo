using AutoMapper;
using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Repositories;
using ezyGo.Core.Exceptions;

namespace ezyGo.Admin.Domain.Managers;

public class RouteStoppageManager : IRouteStoppageManager
{
    private readonly IRouteStoppageRepository _stoppageRepo;
    private readonly IRouteRepository _routeRepo;
    private IBusStationRepository _stationRepo;
    private readonly IMapper _mapper;

    public RouteStoppageManager(IRouteStoppageRepository stoppageRepo, IMapper mapper, IRouteRepository routeRepo, IBusStationRepository stationRepo)
    {
        _stoppageRepo = stoppageRepo;
        _mapper = mapper;
        _routeRepo = routeRepo;
        _stationRepo = stationRepo;
    }
    public async Task<RouteStoppage> AddStoppageEndAsync(RouteStoppage model)
    {
        bool isExist = await _stoppageRepo.IsStoppageAlreadyExist(model.RouteId, model.StationId);

        await ValidateStation(model.StationId);
        await ValidateRoute(model.RouteId);

        if (isExist)
            throw new AlreadyExistException($"The Stoppage: \"{model.StationId}\" is already assigned in the route \"{model.RouteId}\"");

        model.CreatedAt = DateTime.UtcNow;
        model.UpdatedAt = DateTime.UtcNow;

        var stoppageEntity = _mapper.Map<RouteStoppageEntity>(model);
        var stoppage = await _stoppageRepo.InsertAtEndAsync(stoppageEntity);

        return _mapper.Map<RouteStoppage>(stoppage);
    }



    public async Task<RouteStoppage> AddStoppageMiddleAsync(int routeId, int stationId, int insertAfterOrder)
    {
        bool isExist = await _stoppageRepo.IsStoppageAlreadyExist(routeId, stationId);

        if (isExist)
            throw new AlreadyExistException($"The Stoppage: \"{stationId}\" is already assigned in the route \"{routeId}\"");

        await ValidateStation(stationId);
        await ValidateRoute(routeId);

        var stoppage = await _stoppageRepo.InsertInMiddleAsync(routeId, stationId, insertAfterOrder);

        return _mapper.Map<RouteStoppage>(stoppage);
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<RouteStoppage>> GetStoppagesAsync(int routeId)
    {
        var stoppages = await _stoppageRepo.GetByRouteIdAsync(routeId);

        return _mapper.Map<List<RouteStoppage>>(stoppages);
    }

    public Task<bool> ReorderAsync(int routeId, List<int> stationIds)
    {
        throw new NotImplementedException();
    }



    private async Task ValidateStation(int stationId)
    {
        var isExist = await _stationRepo.IsExistsAsync(stationId);

        if (!isExist)
            throw new NotFoundException($"Cannot add it as a stoppage, Station with id: {stationId}");
    }

    private async Task ValidateRoute(int routeId)
    {
        var isExist = await _routeRepo.IsExistsAsync(routeId);

        if (!isExist)
            throw new NotFoundException($"Cannot add stoppage to this route, Route with id: {routeId}");
    }
}
