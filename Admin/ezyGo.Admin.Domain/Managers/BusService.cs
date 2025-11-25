using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using static System.Collections.Specialized.BitVector32;

namespace ezyGo.Admin.Domain.Managers;

public class BusService : IBusService
{
    private readonly IBusRepository _busRepository;

    public BusService(IBusRepository busRepository)
    {
        _busRepository = busRepository;
    }

    public async Task Create(Station station)
    {
        await _busRepository.Create(station);
    }

    public async Task Update(Station station)
    {
        await _busRepository.Update(station);
    }
}
