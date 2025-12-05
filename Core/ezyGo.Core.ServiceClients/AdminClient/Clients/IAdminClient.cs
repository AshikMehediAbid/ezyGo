using ezyGo.Core.ServiceClients.AdminClient.Models;

namespace ezyGo.Core.ServiceClients.AdminClient.Clients;

public interface IAdminClient
{
    Task<List<TripTemplateClientModel>> GetTripTemplateByCompanyId(int companyId);
}
