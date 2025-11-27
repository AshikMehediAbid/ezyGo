using AutoMapper;
using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Repositories;

namespace ezyGo.Admin.Domain.Managers;

public class TrainService : ITrainService
{
    private readonly ITrainRepository _trainRepository;
    private readonly IMapper _mapper;

    public TrainService(ITrainRepository rainRepository, IMapper mapper)
    {
        _trainRepository = rainRepository;
        _mapper = mapper;
    }

    public async Task Create(Station station)
    {
        var stationEntity = _mapper.Map<TrainStation>(station);
        await _trainRepository.Create(stationEntity);
    }
}
