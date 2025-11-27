using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Sql;
using ezyGo.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;

namespace ezyGo.Admin.Storage.Repositories;

public class BusRepository : GenericRepository<BusEntity>, IBusRepository
{
    private readonly AdminDbContext _db;

    public BusRepository(AdminDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<IEnumerable<BusEntity>> GetAllBusesAsync(string? filter)
    {
        var busesQuery = _db.Buses
            .Include(b => b.Company)
            .AsQueryable()
            .AsNoTracking();

        if (!string.IsNullOrEmpty(filter))
        {
            busesQuery = busesQuery.Where(b =>
                b.BusName.Contains(filter) ||
                b.Company.CompanyName.Contains(filter)
            );
        }

        var busEntities = await busesQuery.ToListAsync();
        return busEntities;
    }
}
