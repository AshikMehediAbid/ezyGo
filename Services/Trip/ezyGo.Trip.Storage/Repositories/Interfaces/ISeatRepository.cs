using ezyGo.EntityFrameworkCore.Repository;
using ezyGo.Trip.Storage.Entities;

namespace ezyGo.Trip.Storage.Repositories.Interfaces;

public interface ISeatRepository : IGenericRepository<Seat> 
{
    Task<List<Seat>> GetAllSeatByTripId(int tripId);
    Task DeleteSeatByTripId(int tripId);

}
