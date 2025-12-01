using AutoMapper;
using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ezyGo.Admin.Domain.Managers;

public class TripTemplateService : ITripTemplateService
{
    private readonly ITripTemplateRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<TripTemplateService> _logger;

    public TripTemplateService(ITripTemplateRepository repo, IMapper mapper, ILogger<TripTemplateService> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
    }


    public async Task<TripTemplateModel> CreateTripTemplateAsync(TripTemplateModel tripTemplate)
    {
        if (tripTemplate.Route == null)
        {
            _logger.LogWarning("Route information was not supplied when creating a trip template.");
            throw new ArgumentException("Route cannot be null.");
        }

        tripTemplate.Stoppages = GetStoppages(tripTemplate.Route.Stoppages);

        var tripTemplateEntity = _mapper.Map<TripTemplate>(tripTemplate);

        var trip = await _repo.AddAsync(tripTemplateEntity);

        return _mapper.Map<TripTemplateModel>(trip);
    }

    private string GetStoppages(IList<RouteStoppage> stoppages)
    {
        if (stoppages == null || stoppages.Count == 0)
            return string.Empty;

        string stopp = string.Join(" - ", stoppages
            .Where(s => s.Station != null)
            .OrderBy(s => s.Order)
            .Select(s => s.Station!.StationName));

        return stopp;
    }
}
