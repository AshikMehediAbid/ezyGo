using ezyGo.EntityFrameworkCore.Repository;
using ezyGo.Trip.Storage.Entities;
using ezyGo.Trip.Storage.Repositories.Interfaces;

namespace ezyGo.Trip.Storage.Repositories;

public class TripRepository : GenericRepository<TemplateTrip>, ITripRepository
{
    private readonly TripDbContext _context;
    public TripRepository(TripDbContext context) : base(context)
    {
    }
    public Task<List<TemplateTrip>> GetTemplateTripByCompanyId(int companyId)
    {
        throw new NotImplementedException();
    }
}
