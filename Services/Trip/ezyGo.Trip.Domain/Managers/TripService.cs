using AutoMapper;
using ezyGo.Core.ServiceClients.AdminClient.Clients;
using ezyGo.Trip.Domain.Managers.Interface;
using ezyGo.Trip.Domain.Models;

namespace ezyGo.Trip.Domain.Managers;

public class TripService : ITripService
{
    private readonly IAdminClient _adminClient;
    private readonly IMapper _mapper;

    public TripService(IAdminClient adminClient, IMapper mapper )
    {
        _adminClient = adminClient;
        _mapper = mapper;
    }

    public async Task<List<TemplateTripResponse>> GetTripTemplateByCompanyId(int companyId)
    {
        var TreapTemplate = await _adminClient.GetTripTemplateByCompanyId(companyId);
        
        return _mapper.Map<List<TemplateTripResponse>>(TreapTemplate);
    }
}
