using ezyGo.Core.ServiceClients.AdminClient.Clients;
using ezyGo.Trip.Domain.Managers;
using ezyGo.Trip.Domain.Managers.Interface;
using ezyGo.Trip.Domain.Mapping;
using ezyGo.Trip.Storage.Repositories;
using ezyGo.Trip.Storage.Repositories.Interfaces;
using ezyGo.Trip.Storage.Sql;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace ezyGo.Trip.Service.Configuration;

public static class DependencyConfig
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // register other services and repositories here as needed
        services.AddScoped<ITripService, TripService>();
        services.AddScoped<ITripRepository, TripRepository>();


        // Admin Service integration with resilience
        services.AddHttpClient<IAdminClient, AdminClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:Admin"]!);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddStandardResilienceHandler(options =>
            {
                options.Retry.MaxRetryAttempts = 3;
                options.Retry.Delay = TimeSpan.FromSeconds(1);
                options.Retry.BackoffType = DelayBackoffType.Exponential;
                options.Retry.UseJitter = true;
                options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
                options.CircuitBreaker.MinimumThroughput = 5;
                options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(15);
                options.CircuitBreaker.FailureRatio = 0.5;
                options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(20);
            });


        // AutoMapper
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

        // Initialize DB
        var connectionString = configuration["ConnectionStrings:DefaultConnection"];
        services.AddDbContext<TripDbContext>(options =>
        options.UseSqlServer(connectionString)
        );
    }
}
