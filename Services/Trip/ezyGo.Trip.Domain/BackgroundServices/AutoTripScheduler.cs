using AutoMapper;
using ezyGo.Trip.Domain.Managers.Interface;
using ezyGo.Trip.Domain.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly.Caching;
using System.ComponentModel.Design;

namespace ezyGo.Trip.Domain.BackgroundServices;

public class AutoTripScheduler : BackgroundService
{
    private readonly ILogger<AutoTripScheduler> _logger;
    private readonly ITripService _tripService;
    private readonly IMapper _mapper;

    public AutoTripScheduler(ILogger<AutoTripScheduler> logger, ITripService tripService, IMapper mapper)
    {
        _logger = logger;
        _tripService = tripService;
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

            _logger.LogInformation($"Next auto-trip scheduling at: {nextRun}");

            await Task.Delay(delay, stoppingToken);

            await GenerateTripsAutomatically(stoppingToken);

        }
    }



    private DateTime GetNextRunTime()
    {
        var today = DateTime.Now;
        var nextDay = today.Date.AddDays(0).AddHours(0).AddMinutes(2); // 12:01 AM
        return nextDay;
    }


    private async Task GenerateTripsAutomatically(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Starting auto trip scheduling job...");
            var templates = await _tripService.GetAllTripTemplate();

            foreach (var template in templates)
            {
                if(!template.IsAutoScheduled)
                    continue;

                var tripDetails = _mapper.Map<TripDetailsModel>(template);
                tripDetails.TravelDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7)); // Set travel date

                var result = await _tripService.ScheduleTrip(tripDetails);

                _logger.LogInformation($"Auto-Trip created for Template ID: {template.TripTemplateId}");
            }

            _logger.LogInformation("Auto trip scheduling job completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while generating trips automatically.");
        }
    }
}
