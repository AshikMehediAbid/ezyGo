using ezyGo.Trip.Domain.Models;

namespace ezyGo.Trip.Domain.Managers.Interface;

public interface ITripService
{
    public Task<List<TemplateTripResponse>> GetTripTemplateByCompanyId(int companyId);
}
