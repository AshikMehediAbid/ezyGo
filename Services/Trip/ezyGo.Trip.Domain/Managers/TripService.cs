using AutoMapper;
using ezyGo.Core.ServiceClients.AdminClient.Clients;
using ezyGo.Trip.Domain.Managers.Interface;
using ezyGo.Trip.Domain.Models;
using ezyGo.Trip.Storage.Entities;
using ezyGo.Trip.Storage.Repositories.Interfaces;

namespace ezyGo.Trip.Domain.Managers;

public class TripService : ITripService
{
    private readonly ITripRepository _tripRepository;
    private readonly IAdminClient _adminClient;
    private readonly IMapper _mapper;

    public TripService(IAdminClient adminClient, IMapper mapper, ITripRepository tripRepository )
    {
        _adminClient = adminClient;
        _mapper = mapper;
        _tripRepository = tripRepository;
    }

    public async Task<List<TemplateTripResponse>> GetTripTemplateByCompanyId(int companyId)
    {
        var TreapTemplate = await _adminClient.GetTripTemplateByCompanyId(companyId);
        
        return _mapper.Map<List<TemplateTripResponse>>(TreapTemplate);
    }

    public async Task<TripDetailsModel> ScheduleTrip(TripDetailsModel tripDetails)
    {
        var scheduledTrip = _mapper.Map<TripDetails>(tripDetails);
        var scheduledTripEntity =   await _tripRepository.AddAsync(scheduledTrip);

        return _mapper.Map<TripDetailsModel>(scheduledTripEntity);
    }
}
