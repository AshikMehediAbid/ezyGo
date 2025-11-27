using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Sql;
using ezyGo.EntityFrameworkCore.Repository;

namespace ezyGo.Admin.Storage.Repositories;

public class RouteRepository : GenericRepository<RouteEntity> , IRouteRepository
{
    private readonly AdminDbContext _db;

    public RouteRepository(AdminDbContext db) : base(db)
    {
        _db = db;
    }
}
