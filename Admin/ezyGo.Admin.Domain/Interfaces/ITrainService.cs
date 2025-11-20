using ezyGo.Admin.Domain.Models;

namespace ezyGo.Admin.Domain.Interfaces;

public interface ITrainService
{
    Task Create(Station train);
}
