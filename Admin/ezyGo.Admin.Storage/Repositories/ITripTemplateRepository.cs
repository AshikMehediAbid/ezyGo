using ezyGo.Admin.Storage.Entities;
using ezyGo.EntityFrameworkCore.Repository;

namespace ezyGo.Admin.Storage.Repositories;

public interface ITripTemplateRepository : IGenericRepository<TripTemplate>
{
    Task<IEnumerable<TripTemplate>> GetTripTemplatesWithDetailsAsync(string? filter);
    Task<TripTemplate?> GetTripTemplateByIdWithDetailsAsync(int id);
    Task<List<TripTemplate?>> GetTripTemplatesByCompanyIdAsync(int companyId);
    Task<bool> IsTripTemplateExistAsync(int routeId, int? busId, int baseFare, TimeOnly departureTime, TimeOnly arrivalTime);
}
