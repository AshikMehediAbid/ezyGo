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

}
