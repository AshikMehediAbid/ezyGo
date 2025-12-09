using AutoMapper;
using ezyGo.Core.Exceptions;
using ezyGo.Trip.Domain.Managers.Interface;
using ezyGo.Trip.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ezyGo.Trip.Domain.BackgroundServices;

public class AutoTripScheduler : BackgroundService
{
    private readonly ILogger<AutoTripScheduler> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public AutoTripScheduler(ILogger<AutoTripScheduler> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("AutoTripScheduler started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            // Schedule next run at 12:01 AM
            var nextRun = GetNextRunTime();
            var delay = nextRun - DateTime.Now;
            // Safety check for negative delay
            if (delay < TimeSpan.Zero)
            {
                _logger.LogWarning("NextRun time already passed. Running immediately.");
                delay = TimeSpan.Zero;
            }

            _logger.LogInformation($"Next auto-trip scheduling at: {nextRun}");

            await Task.Delay(delay, stoppingToken);

            _logger.LogInformation("Auto-trip scheduling triggered.");
            await GenerateTripsAutomatically(stoppingToken);

        }
    }



    private DateTime GetNextRunTime()
    {
        var today = DateTime.Now;
        var nextDay = today.Date.AddDays(1).AddHours(0).AddMinutes(1); // 12:01 AM
        //var nextDay = today.AddSeconds(30); // For testing purpose only
        return nextDay;
    }


    private async Task GenerateTripsAutomatically(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Starting auto trip scheduling job...");

            // Create scope manually for scoped services
            using var scope = _scopeFactory.CreateScope();

            var _tripService = scope.ServiceProvider.GetRequiredService<ITripService>();

            var templates = await _tripService.GetAllTripTemplate();

            foreach (var template in templates)
            {
                try
                {
                    if (!template.IsAutoScheduled)
                        continue;

                    var tripDetails = _mapper.Map<TripDetailsModel>(template);
                    tripDetails.TravelDate = DateOnly.FromDateTime(DateTime.Now.AddDays(6)); // Set travel date

                    var result = await _tripService.ScheduleTrip(tripDetails);

                    _logger.LogInformation($"Auto-Trip created for Template ID: {template.TripTemplateId}");
                }
                catch (AlreadyExistException aex)
                {
                    _logger.LogWarning(aex, $"Trip already exists for Template ID: {template.TripTemplateId}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error scheduling trip for Template ID: {template.TripTemplateId}");
                }
            }

            _logger.LogInformation("Auto trip scheduling job completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while generating trips automatically.");
        }
    }
}
