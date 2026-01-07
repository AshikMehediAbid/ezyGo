using ezyGo.Core.ServiceClients.AdminClient.Models;

namespace ezyGo.Core.ServiceClients.AdminClient.Clients;

public interface IAdminClient
{
    Task<List<TripTemplateClientResponse>> GetAllTripTemplate();
    Task<List<TripTemplateClientResponse>> GetTripTemplateByCompanyId(int companyId);

    Task<List<string>> GetAllCounter(string filter);
}
