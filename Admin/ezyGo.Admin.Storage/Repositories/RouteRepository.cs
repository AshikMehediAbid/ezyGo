using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Sql;
using ezyGo.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;

namespace ezyGo.Admin.Storage.Repositories;

public class RouteRepository : GenericRepository<RouteEntity>, IRouteRepository
{
    private readonly AdminDbContext _db;

    public RouteRepository(AdminDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<RouteEntity> CreateRouteAsync(RouteEntity route)
    {
        await _db.Routes.AddAsync(route);
        await _db.SaveChangesAsync();

        var createdRoute = await _db.Routes
            .Include(r => r.StartingPoint)
            .Include(r => r.EndingPoint)
            .Include(r => r.Stoppages)
                .ThenInclude(s => s.BusStationEntity)
            .FirstOrDefaultAsync(r => r.Id == route.Id);

        return createdRoute!;
    }

    public async Task<List<RouteEntity>> GetRoutesAsync(string? filter)
    {
        var routesQuery = _db.Routes
            .Include(r=>r.StartingPoint)
            .Include(r=>r.EndingPoint)
            .Include(r => r.Stoppages)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter))
        {
            routesQuery = routesQuery.Where(r =>
                r.StartingPoint.StationName.Contains(filter) ||
                r.EndingPoint.StationName.Contains(filter));
        }

        var routesEntities = await routesQuery.ToListAsync();
        return routesEntities;
    }

    public async Task<bool> IsRouteExist(int startId, int endId)
    {
        var isExist = await _db.Routes
            .AnyAsync(r => r.StartingPointId == startId && r.EndingPointId == endId);

        return isExist;
    }
}
