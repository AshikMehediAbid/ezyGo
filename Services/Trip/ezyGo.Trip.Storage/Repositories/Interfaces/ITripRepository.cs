using ezyGo.EntityFrameworkCore.Repository;
using ezyGo.Trip.Storage.Entities;

namespace ezyGo.Trip.Storage.Repositories.Interfaces;

public interface ITripRepository : IGenericRepository<TripDetails>
{
    Task<List<TripDetails>> GetAllTripByUserSearchRequest(TripRequest filter);
    Task<List<TripDetails>> GetAllTripByDateAsync(DateOnly date);
    public Task<List<TemplateTrip>> GetTemplateTripByCompanyId(int companyId);
    Task<bool> IsTripExistAsync(int templateId, DateOnly tripDate);
}
