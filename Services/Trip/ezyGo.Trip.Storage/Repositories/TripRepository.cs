using ezyGo.EntityFrameworkCore.Repository;
using ezyGo.Trip.Storage.Entities;
using ezyGo.Trip.Storage.Repositories.Interfaces;
using ezyGo.Trip.Storage.Sql;
using Microsoft.EntityFrameworkCore;

namespace ezyGo.Trip.Storage.Repositories;

public class TripRepository : GenericRepository<TripDetails>, ITripRepository
{
    private readonly TripDbContext _db;
    public TripRepository(TripDbContext db) : base(db)
    {
        _db = db;
    }

    public Task<List<TripDetails>> GetAllTripByDateAsync(DateOnly date)
    {
        var query = _db.TripDetails.AsQueryable().AsNoTracking();

        query = query.Where(t => t.TravelDate < date);

        return query.ToListAsync();
    }

    public Task<List<TripDetails>> GetAllTripByUserSearchRequest(TripRequest filter)
    {
        var query = _db.TripDetails.AsQueryable().AsNoTracking();

        query = query.Where(t =>
            t.TravelDate == filter.TripDate &&
            t.Stoppages != null &&
            t.Stoppages.Contains(filter.FromLocation) &&
            t.Stoppages.Contains(filter.ToLocation) &&
            t.Stoppages.IndexOf(filter.FromLocation) < t.Stoppages.IndexOf(filter.ToLocation)
        );

        return query.ToListAsync();
    }

    public Task<List<TemplateTrip>> GetTemplateTripByCompanyId(int companyId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsTripExistAsync(int templateId, DateOnly tripDate)
    {
        var query = _db.TripDetails.AsQueryable();

        query = query.Where(t =>
            t.TripTemplateId == templateId &&
            t.TravelDate == tripDate);

        return await query.AnyAsync();

    }
}
