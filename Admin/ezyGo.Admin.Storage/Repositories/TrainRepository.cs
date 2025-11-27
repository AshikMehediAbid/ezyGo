using AutoMapper;
using ezyGo.Admin.Storage.Entities;
using ezyGo.Admin.Storage.Sql;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ezyGo.Admin.Storage.Repositories;

public class TrainRepository : ITrainRepository
{
    private readonly IMapper _mapper;
    private readonly AdminDbContext _db;
    public TrainRepository(IMapper mapper, AdminDbContext db)
    {
        _mapper = mapper;
        _db = db;
    }

    public async Task Create(TrainStation station)
    {
        var trainStationEntity = _mapper.Map<TrainStation>(station);

        await _db.TrainStations.AddAsync(trainStationEntity);
        await _db.SaveChangesAsync();
    }
}
