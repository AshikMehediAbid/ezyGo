using ezyGo.Core.ServiceClients.PaymentClient.Clients;
using ezyGo.Core.ServiceClients.TripClient.Clients;
using ezyGo.PdfGenerator.Services;
using ezyGo.Platform.Domain.DistributedLock;
using ezyGo.Platform.Domain.Managers;
using ezyGo.Platform.Domain.Managers.Interfaces;
using Polly;
using QuestPDF.Infrastructure;
using StackExchange.Redis;

namespace ezyGo.Platform.Service.Configuration;

public static class DependencyConfig
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {

        // Service register
        services.AddScoped<ITripService, TripService>();

        services.AddScoped<ISeatService, SeatService>();
        services.AddScoped<ILockService, LockService>();

        services.AddScoped<IUtilityService, UtilityService>();

        // Configure redis
        var redisConnectionString = configuration.GetConnectionString("Redis");
        
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        ConnectionMultiplexer.Connect(redisConnectionString!));

        // Trip Service integration with resilience
        services.AddHttpClient<ITripClient, TripClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:Trip"]!);
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

        // Payment Service integration with resilience
        services.AddHttpClient<IPaymentClient, PaymentClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:Payment"]!);
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

        // PDF Generation Service
        services.AddScoped<ITicketPdfService, TicketPdfService>();
    }
}
