using ezyGo.Trip.Domain.Models;

namespace ezyGo.Trip.Domain.Managers.Interface;

public interface ITripService
{
    Task<List<TemplateTripResponse>> GetAllTripTemplate();
    Task<List<TemplateTripResponse>> GetTripTemplateByCompanyId(int companyId);
    Task<TripDetailsModel> ScheduleTrip(TripDetailsModel tripDetails);
}
