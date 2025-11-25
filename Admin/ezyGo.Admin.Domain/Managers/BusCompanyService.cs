using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using ezyGo.Core.Exceptions;

namespace ezyGo.Admin.Domain.Managers;

public class BusCompanyService : IBusCompanyService
{
    private readonly IBusCompanyRepository _busCompanyRepo;

    public BusCompanyService(IBusCompanyRepository busCompanyRepo)
    {
        _busCompanyRepo = busCompanyRepo;
    }
    public async Task<BusCompany> CreateBusCompanyAsync(BusCompany company)
    {
        company.CreatedAt = DateTime.UtcNow;
        return await _busCompanyRepo.AddAsync(company);
    }

    public async Task DeleteBusCompanyAsync(int id)
    {
        var company = await _busCompanyRepo.GetByIdAsync(id);
        if (company == null)
            throw new NotFoundException("Bus Company");

        await _busCompanyRepo.DeleteAsync(company);
    }

    public async Task<IEnumerable<BusCompany>> GetAllBusCompaniesAsync()
    {
        return await _busCompanyRepo.GetAllAsync();
    }

    public async Task<BusCompany?> GetBusCompanyByIdAsync(int id)
    {
        return await _busCompanyRepo.GetByIdAsync(id);
    }

    public async Task UpdateBusCompanyAsync(int id, BusCompany company)
    {
        var existingCompany = await _busCompanyRepo.GetByIdAsync(id);
        if (existingCompany == null)
            throw new NotFoundException("Bus Company");

        existingCompany.CompanyName = company.CompanyName;
        existingCompany.OwnerName = company.OwnerName;
        existingCompany.RegistrationNo = company.RegistrationNo;

        await _busCompanyRepo.UpdateAsync(existingCompany);
    }
}
