using ezyGo.Admin.Domain.Models;

namespace ezyGo.Admin.Domain.Interfaces;

public interface ITripTemplateService
{
    Task<TripTemplateModel> CreateTripTemplateAsync(TripTemplateModel tripTemplate);
    Task<IEnumerable<TripTemplateModel>> GetTripTemplatesAsync(string? filter);
    Task<TripTemplateModel> GetTripTemplateByIdAsync(int id);
    Task UpdateTripTemplateAsync(int id, TripTemplateModel tripTemplate);
    Task DeleteTripTemplateAsync(int id);
}
