using ezyGo.Admin.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace ezyGo.Admin.Storage.Sql;

public class AdminDbContext : DbContext
{
    public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options) { }

    public DbSet<TrainStation> TrainStations { get; set; }
}
