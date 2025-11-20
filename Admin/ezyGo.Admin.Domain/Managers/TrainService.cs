using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;

namespace ezyGo.Admin.Domain.Managers;

public class TrainService : ITrainService
{
    private readonly ITrainRepository _rainRepository;

    public TrainService(ITrainRepository rainRepository)
    {
        _rainRepository = rainRepository;
    }

    public async Task Create(Station station)
    {
        await _rainRepository.Create(station);
    }
}
