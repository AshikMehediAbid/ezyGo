using ezyGo.Admin.Storage.Entities;
using ezyGo.EntityFrameworkCore.Repository;

namespace ezyGo.Admin.Storage.Repositories;

public interface IBusCompanyRepository : IGenericRepository<BusCompanyEntity>
{
    Task<IEnumerable<BusCompanyEntity>> GetAllBusCompanyAsync(string? filter);
}
