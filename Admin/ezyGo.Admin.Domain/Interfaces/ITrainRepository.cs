using ezyGo.Admin.Domain.Models;

namespace ezyGo.Admin.Domain.Interfaces;

public interface ITrainRepository
{
    Task Create(Station vehicle);
}
