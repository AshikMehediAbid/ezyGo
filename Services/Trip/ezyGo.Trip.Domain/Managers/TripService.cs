using AutoMapper;
using ezyGo.Core.Exceptions;
using ezyGo.Core.ServiceClients.AdminClient.Clients;
using ezyGo.Trip.Domain.Managers.Interface;
using ezyGo.Trip.Domain.Models;
using ezyGo.Trip.Storage.Entities;
using ezyGo.Trip.Storage.Repositories.Interfaces;

namespace ezyGo.Trip.Domain.Managers;

public class TripService : ITripService
{
    private readonly ITripRepository _tripRepository;
    private readonly ISeatService _seatService;
    private readonly IAdminClient _adminClient;
    private readonly IMapper _mapper;

    public TripService(IAdminClient adminClient, IMapper mapper, ITripRepository tripRepository, ISeatService seatService)
    {
        _adminClient = adminClient;
        _mapper = mapper;
        _tripRepository = tripRepository;
        _seatService = seatService;
    }

    public async Task<List<TemplateTripResponse>> GetAllTripTemplate()
    {
        var allTripTemplate = await _adminClient.GetAllTripTemplate();

        return _mapper.Map<List<TemplateTripResponse>>(allTripTemplate);
    }

    public async Task<List<TemplateTripResponse>> GetTripTemplateByCompanyId(int companyId)
    {
        var TreapTemplate = await _adminClient.GetTripTemplateByCompanyId(companyId);

        return _mapper.Map<List<TemplateTripResponse>>(TreapTemplate);
    }

    public async Task<TripDetailsModel> ScheduleTrip(TripDetailsModel tripDetails)
    {
        var isDuplicate = await CheckDuplicateTrip(tripDetails);
        if (isDuplicate)
        {
            throw new AlreadyExistException($"Trip with TemplateId: {tripDetails.TripTemplateId} scheduled for the selected date.{tripDetails.TravelDate}");
        }

        var scheduledTrip = _mapper.Map<TripDetails>(tripDetails);
        var scheduledTripEntity = await _tripRepository.AddAsync(scheduledTrip);

        await _seatService.CreateSeatsForTrip(scheduledTrip.Id, scheduledTrip.TotalCapacity, scheduledTrip.BaseFare);

        return _mapper.Map<TripDetailsModel>(scheduledTripEntity);
    }

    public async Task<IEnumerable<TripDetailsModel>> GetAllTripByUserSearchRequest(UserTripRequest tripRequest)
    {
        var filter = _mapper.Map<TripRequest>(tripRequest);
        var trips = await _tripRepository.GetAllTripByUserSearchRequest(filter);

        return _mapper.Map<IEnumerable<TripDetailsModel>>(trips);
    }


    public async Task<TripDetailsModel?> GetTripById(int id)
    {
        var trip = await _tripRepository.GetByIdAsync(id);
        return trip is null ? null : _mapper.Map<TripDetailsModel>(trip);
    }

    public async Task<TripDetailsModel> UpdateTrip(int id, TripDetailsModel trip)
    {
        var existing = await _tripRepository.GetByIdAsync(id) ?? throw new NotFoundException($"Trip {id} not found");

        // Keep the id consistent
        trip.Id = id;

        var updatedEntity = _mapper.Map<TripDetails>(trip);
        await _tripRepository.UpdateAsync(updatedEntity);

        return _mapper.Map<TripDetailsModel>(updatedEntity);
    }

    public async Task<bool> DeleteTrip(int id)
    {
        var existing = await _tripRepository.GetByIdAsync(id);
        if (existing is null)
        {
            return false;
        }

        await _tripRepository.DeleteAsync(existing);
        await _seatService.DeleteSeatsForTrip(id);
        return true;
    }

    public async Task<bool> DeleteAllTripByTripDate(DateOnly date)
    {
        var trips = await _tripRepository.GetAllTripByDateAsync(date);
        if (trips is null)
        {
            return false;
        }
        foreach (var trip in trips)
        {
            await _tripRepository.DeleteAsync(trip);
        }
        return true;
    }

    private async Task<bool> CheckDuplicateTrip(TripDetailsModel tripDetails)
    {
        var isTripExist = await _tripRepository.IsTripExistAsync(
            tripDetails.TripTemplateId,
            tripDetails.TravelDate);

        return isTripExist;
    }
}
