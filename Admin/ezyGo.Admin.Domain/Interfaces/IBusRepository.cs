using ezyGo.Admin.Domain.Models;

namespace ezyGo.Admin.Domain.Interfaces;

public interface IBusRepository
{
    Task Create(Station busStation);
    Task Update(Station busStation);
}
