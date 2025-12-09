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
