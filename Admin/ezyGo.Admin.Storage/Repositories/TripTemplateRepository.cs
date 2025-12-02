using System.Linq;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Sql;
using ezyGo.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;

namespace ezyGo.Admin.Storage.Repositories;

public class TripTemplateRepository : GenericRepository<TripTemplate>, ITripTemplateRepository
{
    private readonly AdminDbContext _db;

    public TripTemplateRepository(AdminDbContext db) : base(db)
    {
        _db = db;
    }

    private IQueryable<TripTemplate> BuildTripTemplateDetailsQuery()
    {
        return _db.TripTemplates
            .Include(t => t.BusEntity)
                .ThenInclude(b => b.Company)
            .Include(t => t.RouteEntity)
                .ThenInclude(r => r.StartingPoint)
            .Include(t => t.RouteEntity)
                .ThenInclude(r => r.EndingPoint)
            .Include(t => t.RouteEntity)
                .ThenInclude(r => r.Stoppages)
                    .ThenInclude(s => s.BusStationEntity);
    }

    public async Task<IEnumerable<TripTemplate>> GetTripTemplatesWithDetailsAsync(string? filter)
    {
        var query = BuildTripTemplateDetailsQuery()
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(t =>
                t.Description.Contains(filter) ||
                (t.BusEntity != null && t.BusEntity.BusName.Contains(filter)) ||
                (t.RouteEntity != null &&
                    (
                        (t.RouteEntity.StartingPoint != null && t.RouteEntity.StartingPoint.StationName.Contains(filter)) ||
                        (t.RouteEntity.EndingPoint != null && t.RouteEntity.EndingPoint.StationName.Contains(filter))
                    )
                )
            );
        }

        return await query.ToListAsync();
    }

    public async Task<TripTemplate?> GetTripTemplateByIdWithDetailsAsync(int id)
    {
        return await BuildTripTemplateDetailsQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<bool> IsTripTemplateExistAsync(int routeId, int? busId, int baseFare, TimeOnly departureTime, TimeOnly arrivalTime)
    {
        var query = _db.TripTemplates.AsQueryable();

        query = query.Where(t =>
            t.RouteId == routeId &&
            t.BusId == busId.Value &&
            t.BaseFare == baseFare &&
            t.DepartureTime == departureTime &&
            t.ArrivalTime == arrivalTime);

        return await query.AnyAsync();
    }
}
