using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using static System.Collections.Specialized.BitVector32;

namespace ezyGo.Admin.Domain.Managers;

public class BusService : IBusService
{
    private readonly IBusStationRepository _busRepository;

    public BusService(IBusStationRepository busRepository)
    {
        _busRepository = busRepository;
    }

    public async Task Create(Station station)
    {
        await _busRepository.Create(station);
    }

    public async Task Delete(int id)
    {
        await _busRepository.Delete(id);
    }

    public async Task<Station> GetById(int id)
    {
        var station = await _busRepository.GetById(id);

        if (station == null)
            throw new Exception("Station not found!");

        return station;
    }

    public async Task<List<Station>> GetStations(string? filter)
    {
        var stations = await _busRepository.GetStations(filter);

        return stations;
    }

    public async Task Update(Station station)
    {
        await _busRepository.Update(station);
    }
}
