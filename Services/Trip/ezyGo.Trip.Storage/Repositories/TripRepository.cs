using ezyGo.EntityFrameworkCore.Repository;
using ezyGo.Trip.Storage.Entities;
using ezyGo.Trip.Storage.Repositories.Interfaces;
using ezyGo.Trip.Storage.Sql;

namespace ezyGo.Trip.Storage.Repositories;

public class TripRepository : GenericRepository<TemplateTrip>, ITripRepository
{
    public TripRepository(TripDbContext context) : base(context)
    {
    }
    public Task<List<TemplateTrip>> GetTemplateTripByCompanyId(int companyId)
    {
        throw new NotImplementedException();
    }
}
