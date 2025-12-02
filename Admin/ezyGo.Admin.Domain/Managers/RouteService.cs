using AutoMapper;
using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Repositories;
using ezyGo.Core.Exceptions;

namespace ezyGo.Admin.Domain.Managers;

public class RouteService : IRouteService
{
    private readonly IRouteRepository _routeRepo;
    private readonly IMapper _mapper;
    private readonly IRouteStoppageManager _stoppageManager;
    private readonly IBusStationRepository _stationRepo;
    public RouteService(IRouteRepository routeRepo, IMapper mapper, IRouteStoppageManager stoppageManager, IBusStationRepository stationRepo)
    {
        _routeRepo = routeRepo;
        _mapper = mapper;
        _stoppageManager = stoppageManager;
        _stationRepo = stationRepo;
    }

    public async Task<Route> CreateRouteAsync(Route route)
    {
        if (route.StartingPoint == null || route.EndingPoint == null)
            throw new ArgumentException("Route must include both starting and ending stations.");

        await ValidateStations(route);

        var isExist = await _routeRepo.IsRouteExist(route.StartingPoint.Id, route.EndingPoint.Id);

        if (isExist)
            throw new AlreadyExistException($"The route \"{route.StartingPoint.StationName}\" - \"{route.EndingPoint.StationName}\"");

        route.CreatedAt = DateTime.UtcNow;
        route.UpdatedAt = DateTime.UtcNow;

        var routeEntity = _mapper.Map<RouteEntity>(route);

        var createdRoute = await _routeRepo.CreateRouteAsync(routeEntity);

        await AddStartingStoppageAsync(createdRoute);
        await AddEndingStoppageAsync(createdRoute);

        var routeWithDetails = await _routeRepo.GetRouteByIdWithDetailsAsync(createdRoute.Id) ?? createdRoute;

        return _mapper.Map<Route>(routeWithDetails);
    }

    private async Task ValidateStations(Route route)
    {
        var isStartingStationExist = await _stationRepo.IsExistsAsync(route.StartingPoint.Id);
        var isEndingStationExist = await _stationRepo.IsExistsAsync(route.EndingPoint.Id);

        if (!isStartingStationExist || !isEndingStationExist)
            throw new NotFoundException("To create Route, Start Or End Station");
    }

    private async Task AddStartingStoppageAsync(RouteEntity createdRoute)
    {
        var startStoppage = new RouteStoppage
        {
            Id = 0,
            RouteId = createdRoute.Id,
            StationId = createdRoute.StartingPointId ?? 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _stoppageManager.AddStoppageEndAsync(startStoppage);
    }

    private async Task AddEndingStoppageAsync(RouteEntity createdRoute)
    {
        var endStoppage = new RouteStoppage
        {
            Id = 0,
            RouteId = createdRoute.Id,
            StationId = createdRoute.EndingPointId ?? 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _stoppageManager.AddStoppageEndAsync(endStoppage);
    }

    public async Task DeleteRouteAsync(int id)
    {
        var routeEntity = await _routeRepo.GetByIdAsync(id);
        if (routeEntity == null)
            throw new NotFoundException($"Route with id {id} not found");

        await _routeRepo.DeleteAsync(routeEntity);
    }

    public async Task<Route> GetRouteByIdAsync(int id)
    {
        var routeEntity = await _routeRepo.GetRouteByIdWithDetailsAsync(id);
        if (routeEntity == null)
            throw new NotFoundException($"Route with id {id} not found");

        return _mapper.Map<Route>(routeEntity);
    }

    public async Task<IEnumerable<Route>> GetRoutesAsync(string? filter)
    {
        List<RouteEntity> routes = await _routeRepo.GetRoutesAsync(filter);
        return _mapper.Map<IEnumerable<Route>>(routes);
    }

    public async Task UpdateRouteAsync(Route route)
    {
        if (route.StartingPoint == null || route.EndingPoint == null)
            throw new ArgumentException("Route must include both starting and ending stations.");

        await ValidateStations(route);

        route.UpdatedAt = DateTime.UtcNow;
        var routeEntity = _mapper.Map<RouteEntity>(route);
        await _routeRepo.UpdateAsync(routeEntity);
    }

}
