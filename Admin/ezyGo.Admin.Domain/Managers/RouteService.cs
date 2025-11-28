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
    public RouteService(IRouteRepository routeRepo, IMapper mapper)
    {
        _routeRepo = routeRepo;
        _mapper = mapper;
    }

    public async Task<Route> CreateRouteAsync(Route route)
    {
        var isExist = await _routeRepo.IsRouteExist(route.StartingPoint.id, route.EndingPoint.id);

        if (isExist)
            throw new AlreadyExistException($"The route \"{route.StartingPoint.StationName}\" - \"{route.EndingPoint.StationName}\"");

        var routeEntity = _mapper.Map<RouteEntity>(route);

        var createdRoute = await _routeRepo.CreateRouteAsync(routeEntity);

        return _mapper.Map<Route>(createdRoute);
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
        var routeEntity = await _routeRepo.GetByIdAsync(id);
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
        var routeEntity = _mapper.Map<RouteEntity>(route);
        await _routeRepo.UpdateAsync(routeEntity);
    }

}
