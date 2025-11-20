namespace ezyGo.Search.Service.Configuration;

public static class DependencyConfig
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var adminServiceUrl = configuration["Services:Admin"];
        if (string.IsNullOrWhiteSpace(adminServiceUrl))
        {
            throw new ArgumentException("Configuration value for 'Services:Admin' is missing or empty.");
        }

        services.AddHttpClient<HttpClient>(client =>
            client.BaseAddress = new Uri(adminServiceUrl));
    }
}
