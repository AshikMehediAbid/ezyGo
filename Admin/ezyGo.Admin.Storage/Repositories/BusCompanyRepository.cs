using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Sql;
using ezyGo.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;

namespace ezyGo.Admin.Storage.Repositories;

public class BusCompanyRepository : GenericRepository<BusCompanyEntity>, IBusCompanyRepository
{

    private readonly AdminDbContext _db;

    public BusCompanyRepository(AdminDbContext db) : base(db)
    {
        _db = db;
    }


    public async Task<IEnumerable<BusCompanyEntity>> GetAllBusCompanyAsync(string? filter)
    {
        var busCompanyQuery = _db.BusCompanies
            .Include(bc => bc.Buses)
            .AsQueryable()
            .AsNoTracking();

        if (!string.IsNullOrEmpty(filter))
        {
            busCompanyQuery = busCompanyQuery.Where(bc =>
                bc.CompanyName.Contains(filter) ||
                bc.OwnerName.Contains(filter)
            );
        }

        var busCompanyEntities = await busCompanyQuery.ToListAsync();
        return busCompanyEntities;
    }

    public Task<BusCompanyEntity?> GetBusCompanyByIdAsync(int id)
    {
        var busCompany = _db.BusCompanies
            .Include(bc => bc.Buses)
            .AsNoTracking()
            .FirstOrDefaultAsync(bc => bc.Id == id);

        return busCompany;
    }
}
