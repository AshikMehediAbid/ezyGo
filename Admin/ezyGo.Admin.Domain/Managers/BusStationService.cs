using AutoMapper;
using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Repositories;
using ezyGo.Core.Exceptions;
using static System.Collections.Specialized.BitVector32;

namespace ezyGo.Admin.Domain.Managers;

public class BusStationService : IBusStationService
{
    private readonly IBusStationRepository _busStationRepo;
    private readonly IMapper _mapper;

    public BusStationService(IBusStationRepository busRepository, IMapper mapper)
    {
        _busStationRepo = busRepository;
        _mapper = mapper;
    }


    public async Task CreateBusStationAsync(Station station)
    {
        var busStationEntity = _mapper.Map<BusStationEntity>(station);
        await _busStationRepo.AddAsync(busStationEntity);
    }


    public async Task DeleteBusStationAsync(int id)
    {
        var busStation = await _busStationRepo.GetByIdAsync(id);
        if (busStation == null)
            throw new NotFoundException($"Bus Station with id {id}");

        await _busStationRepo.DeleteAsync(busStation);
    }


    public async Task<Station> GetBusStationByIdAsync(int id)
    {
        var stationEntity = await _busStationRepo.GetByIdAsync(id);

        if (stationEntity == null)
            throw new NotFoundException($"Bus Station with id {id}");

        var station = _mapper.Map<Station>(stationEntity);
        return station;

    }


    public async Task<IEnumerable<Station>> GetBusStationsAsync(string? filter)
    {
        var station = await _busStationRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<Station>>(station);
    }


    public async Task UpdateBusStationAsync(Station station)
    {
        var stationEntity = _mapper.Map<BusStationEntity>(station);
        await _busStationRepo.UpdateAsync(stationEntity);
    }
}
