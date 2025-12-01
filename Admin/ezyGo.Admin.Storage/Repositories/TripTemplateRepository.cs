using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Sql;
using ezyGo.EntityFrameworkCore.Repository;

namespace ezyGo.Admin.Storage.Repositories;

public class TripTemplateRepository : GenericRepository<TripTemplate> , ITripTemplateRepository
{
    private readonly AdminDbContext _db;

    public TripTemplateRepository(AdminDbContext db) : base(db)
    {
        _db = db;
    }
}
