using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Sql;
using Microsoft.EntityFrameworkCore;

namespace ezyGo.Admin.Storage.Repositories;

public class BusCompanyRepository : IBusCompanyRepository
{

    private readonly AdminDbContext _db;

    public BusCompanyRepository(AdminDbContext db)
    {
        _db = db;
    }
    public async Task<BusCompany> AddAsync(BusCompany company)
    {
        var entity = MapToEntity(company);
        _db.BusCompanies.Add(entity);
        await _db.SaveChangesAsync();

        company.CompanyId = entity.CompanyId;
        return company;
    }

    public async Task DeleteAsync(BusCompany company)
    {
        var entity = await _db.BusCompanies
            .FirstOrDefaultAsync(x => x.CompanyId == company.CompanyId);

        if (entity != null)
        {
            _db.BusCompanies.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _db.BusCompanies.AnyAsync(x => x.CompanyId == id);
    }

    public async Task<IEnumerable<BusCompany>> GetAllAsync()
    {
        var entities = await _db.BusCompanies.ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task<BusCompany?> GetByIdAsync(int id)
    {
        var entity = await _db.BusCompanies
           .FirstOrDefaultAsync(x => x.CompanyId == id);

        return entity == null ? null : MapToDomain(entity);
    }

    public async Task UpdateAsync(BusCompany company)
    {
        var entity = await _db.BusCompanies
           .FirstOrDefaultAsync(x => x.CompanyId == company.CompanyId);

        if (entity != null)
        {
            entity.CompanyName = company.CompanyName;
            entity.OwnerName = company.OwnerName;
            entity.RegistrationNo = company.RegistrationNo;

            await _db.SaveChangesAsync();
        }
    }


    private static BusCompany MapToDomain(BusCompanyEntity entity)
    {
        return new BusCompany
        {
            CompanyId = entity.CompanyId,
            CompanyName = entity.CompanyName,
            OwnerName = entity.OwnerName,
            RegistrationNo = entity.RegistrationNo,
            CreatedAt = entity.CreatedAt
        };
    }
    private static BusCompanyEntity MapToEntity(BusCompany domain)
    {
        return new BusCompanyEntity
        {
            CompanyId = domain.CompanyId,
            CompanyName = domain.CompanyName,
            OwnerName = domain.OwnerName,
            RegistrationNo = domain.RegistrationNo,
            CreatedAt = domain.CreatedAt
        };
    }
}
