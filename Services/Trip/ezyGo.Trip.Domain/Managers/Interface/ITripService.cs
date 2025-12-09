using ezyGo.Trip.Domain.Models;

namespace ezyGo.Trip.Domain.Managers.Interface;

public interface ITripService
{
    Task<List<TemplateTripResponse>> GetAllTripTemplate();
    Task<List<TemplateTripResponse>> GetTripTemplateByCompanyId(int companyId);
    Task<TripDetailsModel> ScheduleTrip(TripDetailsModel tripDetails);
    Task<IEnumerable<TripDetailsModel>> GetAllTripByUserSearchRequest(UserTripRequest tripRequest);
    Task<TripDetailsModel?> GetTripById(int id);
    Task<TripDetailsModel> UpdateTrip(int id, TripDetailsModel trip);
    Task<bool> DeleteTrip(int id);
    Task<bool> DeleteAllTripByTripDate(DateOnly date);
}
