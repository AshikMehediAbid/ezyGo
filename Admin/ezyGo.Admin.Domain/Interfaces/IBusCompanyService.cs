using ezyGo.Admin.Domain.Models;

namespace ezyGo.Admin.Domain.Interfaces;

public interface IBusCompanyService
{
    Task<IEnumerable<BusCompany>> GetAllBusCompaniesAsync();
    Task<BusCompany?> GetBusCompanyByIdAsync(int id);
    Task<BusCompany> CreateBusCompanyAsync(BusCompany company);
    Task UpdateBusCompanyAsync(int id, BusCompany company);
    Task DeleteBusCompanyAsync(int id);
}
