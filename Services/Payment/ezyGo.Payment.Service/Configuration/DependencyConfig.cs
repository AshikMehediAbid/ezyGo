using ezyGo.Core.ServiceClients.TripClient.Clients;
using ezyGo.Payment.Storage.Sql;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace ezyGo.Payment.Service.Configuration;

public static class DependencyConfig
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {

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

        // Initialize DB
        var connectionString = configuration["ConnectionStrings:PaymentConnection"] ?? throw new ArgumentException("Connection string for Trip DB is not correct");
        services.AddDbContext<PaymentDbContext>(options =>
        options.UseSqlServer(connectionString)
        );
    }
}
