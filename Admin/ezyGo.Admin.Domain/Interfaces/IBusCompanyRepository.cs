using ezyGo.Admin.Domain.Models;

namespace ezyGo.Admin.Domain.Interfaces;

public interface IBusCompanyRepository
{
    Task<BusCompany?> GetByIdAsync(int id);
    Task<IEnumerable<BusCompany>> GetAllAsync();
    Task<BusCompany> AddAsync(BusCompany company);
    Task UpdateAsync(BusCompany company);
    Task DeleteAsync(BusCompany company);
    Task<bool> ExistsAsync(int id);
}
