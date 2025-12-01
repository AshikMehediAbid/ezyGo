using ezyGo.Admin.Domain.Models;

namespace ezyGo.Admin.Domain.Interfaces;

public interface ITripTemplateService
{
    Task<TripTemplateModel> CreateTripTemplateAsync(TripTemplateModel tripTemplate);
}
