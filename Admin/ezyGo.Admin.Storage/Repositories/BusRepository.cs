using AutoMapper;
using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Sql;
using static System.Collections.Specialized.BitVector32;

namespace ezyGo.Admin.Storage.Repositories;

public class BusRepository : IBusRepository
{
    private readonly IMapper _mapper;
    private readonly AdminDbContext _db;
    public BusRepository(IMapper mapper, AdminDbContext db)
    {
        _mapper = mapper;
        _db = db;
    }
    public async Task Create(Station station)
    {
        var busStationEntity = _mapper.Map<BusStation>(station);

        await _db.BusStations.AddAsync(busStationEntity);
        await _db.SaveChangesAsync();
    }

    public async Task Update(Station station)
    {
        var busStationEntity = _mapper.Map<BusStation>(station);

        _db.BusStations.Update(busStationEntity);
        await _db.SaveChangesAsync();
    }
}
