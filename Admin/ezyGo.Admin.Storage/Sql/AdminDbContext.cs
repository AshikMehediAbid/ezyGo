using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Sql.Mapping;
using Microsoft.EntityFrameworkCore;

namespace ezyGo.Admin.Storage.Sql;

public class AdminDbContext : DbContext
{
    public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options) { }

    public DbSet<TrainStation> TrainStations { get; set; }
    public DbSet<BusStationEntity> BusStations { get; set; }
    public DbSet<BusCompanyEntity> BusCompanies { get; set; }
    public DbSet<BusEntity> Buses { get; set; }
    public DbSet<RouteEntity> Routes { get; set; }
    public DbSet<RouteStoppageEntity> RouteStoppages { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BusEntity>(BusMapping.Configure);
        modelBuilder.Entity<RouteEntity>(RouteMapping.Configure);
        modelBuilder.Entity<RouteStoppageEntity>(RouteStoppageMapping.Configure);
    }
}
