using ezyGo.EntityFrameworkCore.Repository;
using ezyGo.Trip.Storage.Entities;
using ezyGo.Trip.Storage.Repositories.Interfaces;
using ezyGo.Trip.Storage.Sql;
using Microsoft.EntityFrameworkCore;

namespace ezyGo.Trip.Storage.Repositories;

public class SeatRepository : GenericRepository<Seat>, ISeatRepository
{
    public readonly TripDbContext _db;
    public SeatRepository(TripDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task DeleteSeatByTripId(int tripId)
    {
        var seats = _db.Seats.Where(s => s.TripId == tripId);
        _db.Seats.RemoveRange(seats);
        await _db.SaveChangesAsync();
    }

    public Task<List<Seat>> GetAllSeatByTripId(int tripId)
    {
        var seats = _db.Seats.Where(s => s.TripId == tripId).AsNoTracking();
        return seats.ToListAsync();
    }
}
