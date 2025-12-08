using ezyGo.EntityFrameworkCore.Repository;
using ezyGo.Trip.Storage.Entities;

namespace ezyGo.Trip.Storage.Repositories.Interfaces;

public interface ITripRepository : IGenericRepository<TripDetails>
{
    public Task<List<TemplateTrip>> GetTemplateTripByCompanyId(int companyId);
}
