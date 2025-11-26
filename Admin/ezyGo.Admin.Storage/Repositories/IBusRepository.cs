using ezyGo.Admin.Storage.Entities;
using ezyGo.EntityFrameworkCore.Repository;

namespace ezyGo.Admin.Storage.Repositories;

public interface IBusRepository : IGenericRepository<BusEntity>
{
    Task<IEnumerable<BusEntity>> GetAllBusesAsync(string? filter);
}
