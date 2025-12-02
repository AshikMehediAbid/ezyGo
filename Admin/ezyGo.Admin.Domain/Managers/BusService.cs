using AutoMapper;
using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Repositories;
using ezyGo.Core.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ezyGo.Admin.Domain.Managers;

public class BusService : IBusService
{
    private readonly IBusCompanyRepository _busCompanyRepo;
    private readonly IBusRepository _busRepo;
    private readonly IMapper _mapper;

    public BusService(IBusCompanyRepository busCompanyRepo, IBusRepository busRepo, IMapper mapper)
    {
        _busCompanyRepo = busCompanyRepo;
        _busRepo = busRepo;
        _mapper = mapper;
    }

    // ====================  BUS COMPANY  ======================//
    public async Task<BusCompany> CreateBusCompanyAsync(BusCompany company)
    {
        if (string.IsNullOrEmpty(company.CompanyName))
        {
            throw new ArgumentNullException(nameof(company.CompanyName), "Company name can not be null or empty");
        }
        company.CreatedAt = DateTime.UtcNow;
        company.UpdatedAt = DateTime.UtcNow;

        var busCompanyEntity = _mapper.Map<BusCompanyEntity>(company);

        var createdEntity = await _busCompanyRepo.AddAsync(busCompanyEntity);
        return _mapper.Map<BusCompany>(createdEntity);
    }

    public async Task DeleteBusCompanyAsync(int id)
    {
        var company = await _busCompanyRepo.GetByIdAsync(id);
        if (company == null)
            throw new NotFoundException("Bus Company");

        await _busCompanyRepo.DeleteAsync(company);
    }

    public async Task<IEnumerable<BusCompany>> GetAllBusCompaniesAsync(string? filter)
    {
        var companies = await _busCompanyRepo.GetAllBusCompanyAsync(filter);
        return _mapper.Map<IEnumerable<BusCompany>>(companies);
    }

    public async Task<BusCompany?> GetBusCompanyByIdAsync(int id)
    {
        var company = await _busCompanyRepo.GetBusCompanyByIdAsync(id);
        if (company == null)
            throw new NotFoundException($"Bus Company with id {id}");

        return _mapper.Map<BusCompany>(company);
    }

    public async Task UpdateBusCompanyAsync(int id, BusCompany company)
    {
        var existingCompany = await _busCompanyRepo.GetByIdAsync(id);
        if (existingCompany == null)
            throw new NotFoundException("Bus Company");

        existingCompany.CompanyName = company.CompanyName;
        existingCompany.OwnerName = company.OwnerName;
        existingCompany.RegistrationNo = company.RegistrationNo;
        existingCompany.UpdatedAt = DateTime.UtcNow;

        await _busCompanyRepo.UpdateAsync(existingCompany);
    }



    // ====================  BUS ======================//
    public async Task UpdateBusAsync(int id, Bus bus)
    {
        var existingBus = await _busRepo.GetByIdAsync(id);
        if (existingBus == null)
            throw new NotFoundException($"Bus with id: {id}");

        existingBus.BusName = bus.BusName;
        existingBus.DriverName = bus.DriverName;
        existingBus.BusRegNo = bus.BusRegNo;
        existingBus.BusType = bus.BusType;
        existingBus.TotalCapacity = bus.TotalCapacity;
        existingBus.UpdatedAt = DateTime.UtcNow;

        await _busRepo.UpdateAsync(existingBus);
    }



    public async Task<IEnumerable<Bus>> GetAllBusesAsync(string? filter)
    {
        var buses = await _busRepo.GetAllBusesAsync(filter);
        return _mapper.Map<IEnumerable<Bus>>(buses);
    }


    public async Task<Bus?> GetBusByIdAsync(int id)
    {
        var bus = await _busRepo.GetByIdAsync(id);
        return _mapper.Map<Bus>(bus);
    }


    public async Task DeleteBusAsync(int id)
    {
        var bus = await _busRepo.GetByIdAsync(id);
        if (bus == null)
            throw new NotFoundException($"Bus with id: {id}");

        await _busRepo.DeleteAsync(bus);
    }



    public async Task<Bus> CreateBusAsync(Bus bus)
    {
        bus.CreatedAt = DateTime.UtcNow;
        bus.UpdatedAt = DateTime.UtcNow;

        var isBusCompanyExist = await _busCompanyRepo.IsExistsAsync(bus.BusCompanyId);

        if (isBusCompanyExist == false)
            throw new NotFoundException("The selected company for Creating a new Bus");

        var busEntity = _mapper.Map<BusEntity>(bus);

        var createdEntity = await _busRepo.AddAsync(busEntity);
        return _mapper.Map<Bus>(createdEntity);
    }

}
