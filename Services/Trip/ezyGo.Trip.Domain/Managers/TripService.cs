using ezyGo.Trip.Domain.Managers.Interface;
using ezyGo.Trip.Domain.Models;

namespace ezyGo.Trip.Domain.Managers;

public class TripService : ITripService
{
    public Task<List<TemplateTripResponse>> GetTemplateTripByCompanyId(int companyId)
    {
        throw new NotImplementedException();
    }
}
