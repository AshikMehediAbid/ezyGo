using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Managers;
using ezyGo.Admin.Domain.Mapping;
using ezyGo.Admin.Storage.Repositories;
using ezyGo.Admin.Storage.Sql;
using ezyGo.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ezyGo.Admin.Service.Configuration;

public static class DependencyConfig
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITrainRepository, TrainRepository>();
        services.AddScoped<ITrainService, TrainService>();

        services.AddScoped<IBusStationRepository, BusStationRepository>();
        services.AddScoped<IBusStationService, BusStationService>();

        services.AddScoped<IBusCompanyRepository, BusCompanyRepository>();
        services.AddScoped<IBusService, BusService>();

        services.AddScoped<IBusRepository, BusRepository>();
        services.AddScoped<IBusService, BusService>();

        services.AddScoped<IRouteRepository, RouteRepository>();
        services.AddScoped<IRouteService, RouteService>();

        services.AddScoped<IRouteStoppageManager, RouteStoppageManager>();
        services.AddScoped<IRouteStoppageRepository, RouteStoppageRepository>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


        // AutoMapper
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

        // Initialize DB
        var connectionString = configuration["ConnectionStrings:DefaultConnection"];
        services.AddDbContext<AdminDbContext>(options =>
        options.UseSqlServer(connectionString)
        );
    }
}
