using ezyGo.Trip.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace ezyGo.Trip.Storage.Sql;

public class TripDbContext : DbContext
{
    public TripDbContext(DbContextOptions<TripDbContext> options) : base(options)
    {
    }

    public DbSet<TripDetails> TripDetails { get; set; }
    public DbSet<Seat> Seats { get; set; }

}
