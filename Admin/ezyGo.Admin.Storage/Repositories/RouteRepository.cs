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

    private IQueryable<RouteEntity> BuildRouteDetailsQuery()
    {
        return _db.Routes
            .Include(r => r.StartingPoint)
            .Include(r => r.EndingPoint)
            .Include(r => r.Stoppages)
                .ThenInclude(s => s.BusStationEntity);
    }

    public async Task<RouteEntity> CreateRouteAsync(RouteEntity route)
    {
        await _db.Routes.AddAsync(route);
        await _db.SaveChangesAsync();

        var createdRoute = await BuildRouteDetailsQuery()
            .FirstOrDefaultAsync(r => r.Id == route.Id);

        return createdRoute!;
    }

    public async Task<List<RouteEntity>> GetRoutesAsync(string? filter)
    {
        var routesQuery = BuildRouteDetailsQuery()
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter))
        {
            routesQuery = routesQuery.Where(r =>
                (r.StartingPoint != null && r.StartingPoint.StationName.Contains(filter)) ||
                (r.EndingPoint != null && r.EndingPoint.StationName.Contains(filter)));
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

    public async Task<RouteEntity?> GetRouteByIdWithDetailsAsync(int id)
    {
        var route = await BuildRouteDetailsQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);

        return route;
    }
}
