using ezyGo.Core.ServiceClients.AdminClient.Models;

namespace ezyGo.Core.ServiceClients.AdminClient.Clients;

public interface IAdminClient
{
    Task<List<TripTemplateClientResponse>> GetTripTemplateByCompanyId(int companyId);
}
