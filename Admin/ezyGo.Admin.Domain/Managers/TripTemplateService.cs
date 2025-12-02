using AutoMapper;
using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Repositories;
using ezyGo.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace ezyGo.Admin.Domain.Managers;

public class TripTemplateService : ITripTemplateService
{
    private readonly ITripTemplateRepository _tripTemplateRepository;
    private readonly IBusRepository _busRepository;
    private readonly IRouteRepository _routeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<TripTemplateService> _logger;

    public TripTemplateService(
        ITripTemplateRepository tripTemplateRepository,
        IBusRepository busRepository,
        IRouteRepository routeRepository,
        IMapper mapper,
        ILogger<TripTemplateService> logger)
    {
        _tripTemplateRepository = tripTemplateRepository;
        _busRepository = busRepository;
        _routeRepository = routeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<TripTemplateModel> CreateTripTemplateAsync(TripTemplateModel tripTemplate)
    {
        if (tripTemplate.RouteId <= 0)
            throw new ArgumentException("Invalied RouteId.");

        var route = await GetRouteAsync(tripTemplate.RouteId!.Value);
        await CheckIsBusExistsAsync(tripTemplate.BusId);


        var isDuplicate = await CheckDuplicateTrip(tripTemplate);
        if (isDuplicate)
        {
            throw new AlreadyExistException("Trip template");
        }

        tripTemplate.Stoppages = BuildStoppages(route);
        tripTemplate.CreatedAt = DateTime.UtcNow;
        tripTemplate.UpdatedAt = DateTime.UtcNow;

        var tripTemplateEntity = _mapper.Map<TripTemplate>(tripTemplate);
        var createdTemplate = await _tripTemplateRepository.AddAsync(tripTemplateEntity);

        var templateWithDetails = await _tripTemplateRepository.GetTripTemplateByIdWithDetailsAsync(createdTemplate.Id) ?? createdTemplate;
        return _mapper.Map<TripTemplateModel>(templateWithDetails);
    }


    private async Task<bool> CheckDuplicateTrip(TripTemplateModel tripTemplate)
    {
        var isDuplicate = await _tripTemplateRepository.IsTripTemplateExistAsync(
            tripTemplate.RouteId!.Value,
            tripTemplate.BusId,
            tripTemplate.BaseFare,
            tripTemplate.DepartureTime,
            tripTemplate.ArrivalTime);

        return isDuplicate;
    }



    public async Task<IEnumerable<TripTemplateModel>> GetTripTemplatesAsync(string? filter)
    {
        var templates = await _tripTemplateRepository.GetTripTemplatesWithDetailsAsync(filter);
        return _mapper.Map<IEnumerable<TripTemplateModel>>(templates);
    }



    public async Task<TripTemplateModel> GetTripTemplateByIdAsync(int id)
    {
        var template = await _tripTemplateRepository.GetTripTemplateByIdWithDetailsAsync(id);
        if (template == null)
        {
            _logger.LogWarning("Trip template with id {TripTemplateId} was not found.", id);
            throw new NotFoundException($"Trip template with id {id}");
        }

        return _mapper.Map<TripTemplateModel>(template);
    }



    public async Task UpdateTripTemplateAsync(int id, TripTemplateModel tripTemplate)
    {
        var existingTemplate = await _tripTemplateRepository.GetByIdAsync(id);
        if (existingTemplate == null)
        {
            _logger.LogWarning("Trip template with id {TripTemplateId} was not found for update.", id);
            throw new NotFoundException($"Trip template with id {id}");
        }

        if (tripTemplate.RouteId <= 0)
            throw new ArgumentException("Invalied RouteId.");
        await CheckIsBusExistsAsync(tripTemplate.BusId);

        var route = await GetRouteAsync(tripTemplate.RouteId!.Value);

        tripTemplate.Stoppages = BuildStoppages(route);
        tripTemplate.UpdatedAt = DateTime.UtcNow;

        var tripTemplateEntity = _mapper.Map<TripTemplate>(tripTemplate);
        await _tripTemplateRepository.UpdateAsync(tripTemplateEntity);
    }



    public async Task DeleteTripTemplateAsync(int id)
    {
        var template = await _tripTemplateRepository.GetByIdAsync(id);
        if (template == null)
        {
            _logger.LogWarning("Trip template with id {TripTemplateId} was not found for delete.", id);
            throw new NotFoundException($"Trip template with id {id}");
        }

        await _tripTemplateRepository.DeleteAsync(template);
    }



    private async Task<RouteEntity> GetRouteAsync(int routeId)
    {
        var route = await _routeRepository.GetRouteByIdWithDetailsAsync(routeId);
        if (route == null)
        {
            _logger.LogWarning("Route with id {RouteId} was not found.", routeId);
            throw new NotFoundException($"Route with id {routeId}");
        }

        return route;
    }


    private async Task CheckIsBusExistsAsync(int? busId)
    {
        if (!busId.HasValue)
            return;

        var exists = await _busRepository.IsExistsAsync(busId.Value);
        if (!exists)
        {
            _logger.LogWarning("Bus with id {BusId} was not found.", busId);
            throw new NotFoundException($"Bus with id {busId.Value}");
        }
    }


    private static string BuildStoppages(RouteEntity route)
    {
        if (route.Stoppages == null || route.Stoppages.Count == 0)
            return string.Empty;

        var stationNames = route.Stoppages
            .Where(s => s.BusStationEntity != null)
            .OrderBy(s => s.Order)
            .Select(s => s.BusStationEntity!.StationName)
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .ToList();

        return stationNames.Count == 0 ? string.Empty : string.Join(" - ", stationNames);
    }
}
