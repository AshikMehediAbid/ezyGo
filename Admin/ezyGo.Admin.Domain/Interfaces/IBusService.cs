using ezyGo.Admin.Domain.Models;

namespace ezyGo.Admin.Domain.Interfaces;

public interface IBusService
{
    Task Create(Station bus);
    Task Update(Station bus);
}
