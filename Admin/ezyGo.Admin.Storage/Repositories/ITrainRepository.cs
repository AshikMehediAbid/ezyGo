using ezyGo.Admin.Storage.Entities;
using static System.Collections.Specialized.BitVector32;

namespace ezyGo.Admin.Storage.Repositories;

public interface ITrainRepository
{
    Task Create(TrainStation vehicle);
}
