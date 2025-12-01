using ezyGo.Admin.Storage.Entities;
using ezyGo.EntityFrameworkCore.Repository;

namespace ezyGo.Admin.Storage.Repositories;

public interface ITripTemplateRepository : IGenericRepository<TripTemplate>
{
    Task<IEnumerable<TripTemplate>> GetTripTemplatesWithDetailsAsync(string? filter);
    Task<TripTemplate?> GetTripTemplateByIdWithDetailsAsync(int id);
    Task<bool> IsTripTemplateExistAsync(int routeId, int? busId, int baseFare, DateTime departureTime, DateTime arrivalTime);
}
