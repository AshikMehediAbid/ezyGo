using AutoMapper;
using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Sql;
using ezyGo.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace ezyGo.Admin.Storage.Repositories;

public class BusStationRepository : IBusStationRepository
{
    private readonly IMapper _mapper;
    private readonly AdminDbContext _db;
    public BusStationRepository(IMapper mapper, AdminDbContext db)
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

    public async Task Delete(int id)
    {
        var busStationEntity = await _db.BusStations.FindAsync(id);
        if (busStationEntity != null)
        {
            _db.BusStations.Remove(busStationEntity);
            await _db.SaveChangesAsync();
        }
        else
        {
            throw new NotFoundException($"Bus station with id {id}");
        }
    }

    public async Task<Station> GetById(int id)
    {
        var stationEntity = await _db.BusStations.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

        var station = _mapper.Map<Station>(stationEntity);
        return station;
    }

    public async Task<List<Station>> GetStations(string? filter)
    {
        var stationsQuery = _db.BusStations.AsQueryable().AsNoTracking();

        if (!string.IsNullOrEmpty(filter))
        {
            stationsQuery = stationsQuery.Where(s => s.StationName.Contains(filter) || s.StationDescription.Contains(filter));
        }

        var stationEntities = await stationsQuery.ToListAsync();
        return _mapper.Map<List<Station>>(stationEntities);
    }

    public async Task Update(Station station)
    {
        var busStationEntity = _mapper.Map<BusStation>(station);

        _db.BusStations.Update(busStationEntity);
        await _db.SaveChangesAsync();
    }
}
