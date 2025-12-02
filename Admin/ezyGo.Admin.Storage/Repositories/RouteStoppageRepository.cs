using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Sql;
using ezyGo.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;

namespace ezyGo.Admin.Storage.Repositories;

public class RouteStoppageRepository : GenericRepository<RouteStoppageEntity>, IRouteStoppageRepository
{
    private readonly AdminDbContext _db;

    public RouteStoppageRepository(AdminDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<IEnumerable<RouteStoppageEntity>> GetByRouteIdAsync(int routeId)
    {
        var stoppages = await _db.RouteStoppages
            .Include(x => x.BusStationEntity)
            .Where(x => x.RouteEntityId == routeId)
            .OrderBy(x => x.Order)
            .AsNoTracking()
            .ToListAsync();

        return stoppages;
    }

    public async Task<bool> UpdateOrderAsync(int id, int newOrder)
    {
        var stoppage = await _db.RouteStoppages
            .FirstOrDefaultAsync(x => x.Id == id);

        if (stoppage is null)
            return false;

        var routeId = stoppage.RouteEntityId;
        var currentOrder = stoppage.Order;

        // Get max order
        var maxOrder = await _db.RouteStoppages
            .Where(x => x.RouteEntityId == routeId)
            .MaxAsync(x => (int?)x.Order) ?? 0;

        newOrder = Math.Max(1, newOrder);
        newOrder = Math.Min(maxOrder, newOrder);

        if (newOrder == currentOrder)
            return true;

        if (newOrder > currentOrder)
        {
            // decrement between (currentOrder, newOrder]
            var affected = await _db.RouteStoppages
                .Where(x => x.RouteEntityId == routeId &&
                            x.Order > currentOrder &&
                            x.Order <= newOrder)
                .ToListAsync();

            foreach (var s in affected)
                s.Order--;
        }
        else
        {
            // increment between [newOrder, currentOrder)
            var affected = await _db.RouteStoppages
                .Where(x => x.RouteEntityId == routeId &&
                            x.Order >= newOrder &&
                            x.Order < currentOrder)
                .ToListAsync();

            foreach (var s in affected)
                s.Order++;
        }

        stoppage.Order = newOrder;
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var stoppage = await _db.RouteStoppages
            .FirstOrDefaultAsync(x => x.Id == id);

        if (stoppage is null)
            return false;

        var routeId = stoppage.RouteEntityId;
        var order = stoppage.Order;

        // Remove the stoppage
        _db.RouteStoppages.Remove(stoppage);

        // Reorder subsequent stoppages to keep the sequence continuous
        var nextStoppages = await _db.RouteStoppages
            .Where(x => x.RouteEntityId == routeId && x.Order > order)
            .ToListAsync();

        foreach (var s in nextStoppages)
        {
            s.Order--;
        }

        await _db.SaveChangesAsync();

        return true;
    }


    public async Task<RouteStoppageEntity> InsertAtEndAsync(RouteStoppageEntity model)
    {
        var maxOrder = await _db.RouteStoppages
            .Where(x => x.RouteEntityId == model.RouteEntityId)
            .MaxAsync(x => (int?)x.Order) ?? 0;


        var entity = new RouteStoppageEntity
        {
            RouteEntityId = model.RouteEntityId,
            BusStationEntityId = model.BusStationEntityId,
            Order = maxOrder + 1
        };

        _db.RouteStoppages.Add(entity);
        await _db.SaveChangesAsync();
        await _db.Entry(entity).Reference(e => e.BusStationEntity).LoadAsync();

        return entity;
    }

    public async Task<RouteStoppageEntity> InsertInMiddleAsync(int routeId, int stationId, int insertAfter)
    {
        // Get the max existing order
        var maxOrder = await _db.RouteStoppages
            .Where(x => x.RouteEntityId == routeId)
            .MaxAsync(x => (int?)x.Order) ?? 0;

        insertAfter = Math.Min(insertAfter, maxOrder);

        var list = await _db.RouteStoppages
            .Where(x => x.RouteEntityId == routeId && x.Order > insertAfter)
            .ToListAsync();

        foreach (var r in list)
            r.Order++;

        var entity = new RouteStoppageEntity
        {
            RouteEntityId = routeId,
            BusStationEntityId = stationId,
            Order = insertAfter + 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.RouteStoppages.Add(entity);
        await _db.SaveChangesAsync();
        await _db.Entry(entity).Reference(e => e.BusStationEntity).LoadAsync();

        return entity;
    }


    public async Task<bool> IsStoppageAlreadyExist(int routeId, int stationId)
    {
        bool isEist = await _db.RouteStoppages
            .AnyAsync(rs => rs.RouteEntityId == routeId && rs.BusStationEntityId == stationId);

        return isEist;
    }
}
