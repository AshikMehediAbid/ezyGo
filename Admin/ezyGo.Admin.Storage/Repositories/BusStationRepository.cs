using AutoMapper;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Sql;
using ezyGo.Core.Exceptions;
using ezyGo.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace ezyGo.Admin.Storage.Repositories;

public class BusStationRepository : GenericRepository<BusStationEntity>, IBusStationRepository
{
    private readonly IMapper _mapper;
    private readonly AdminDbContext _db;
    public BusStationRepository(IMapper mapper, AdminDbContext db) : base(db)
    {
        _mapper = mapper;
        _db = db;
    }

    public async Task<List<BusStationEntity>> GetStations(string? filter)
    {
        var stationsQuery = _db.BusStations.AsQueryable().AsNoTracking();

        if (!string.IsNullOrEmpty(filter))
        {
            stationsQuery = stationsQuery.Where(s => s.StationName.Contains(filter) || s.StationDescription.Contains(filter));
        }

        var stationEntities = await stationsQuery.ToListAsync();
        return stationEntities;
    }

    public async Task DeleteBusStationWithDependenciesAsync(BusStationEntity station)
    {
        // Step 1: delete all route stoppages that depends on this station
        var stoppagesToDelete = await _db.RouteStoppages
            .Where(rs => rs.BusStationEntityId == station.Id)
            .ToListAsync();

        if (stoppagesToDelete.Any())
        {
            _db.RouteStoppages.RemoveRange(stoppagesToDelete);
        }

        // step 2: delete all routes depends on the station
        var routesToDelete = await _db.Routes
            .Where(r => r.StartingPointId == station.Id || r.EndingPointId == station.Id)
            .ToListAsync();

        if (routesToDelete.Any())
        {
            _db.Routes.RemoveRange(routesToDelete);
        }

        // Finally, delete the station itself
        _db.BusStations.Remove(station);
        await _db.SaveChangesAsync();
    }

    public Task<List<string>> GetStationsNameAsync(string? filter)
    {
        var stationsQuery = _db.BusStations.AsQueryable().AsNoTracking();
        if (!string.IsNullOrEmpty(filter))
        {
            stationsQuery = stationsQuery.Where(s => s.StationName.Contains(filter));
        }
        return stationsQuery
            .Select(s => s.StationName)
            .ToListAsync();
    }
}
