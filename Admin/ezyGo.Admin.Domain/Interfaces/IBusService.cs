using ezyGo.Admin.Domain.Models;

namespace ezyGo.Admin.Domain.Interfaces;

public interface IBusService
{
    //=============== Bus Company ====================//
    Task<IEnumerable<BusCompany>> GetAllBusCompaniesAsync(string? filter);
    Task<BusCompany?> GetBusCompanyByIdAsync(int id);
    Task<BusCompany> CreateBusCompanyAsync(BusCompany company);
    Task UpdateBusCompanyAsync(int id, BusCompany company);
    Task DeleteBusCompanyAsync(int id);
 

    //=============== Bus ====================//
    Task<IEnumerable<Bus>> GetAllBusesAsync(string? filter);
    Task<Bus?> GetBusByIdAsync(int id);
    Task<Bus> CreateBusAsync(Bus bus);
    Task UpdateBusAsync(int id, Bus bus);
    Task DeleteBusAsync(int id);
}
