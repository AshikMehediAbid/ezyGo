using ezyGo.Core.ServiceClients.TripClient.Clients;
using ezyGo.Payment.Domain.Managers.Interfaces;

namespace ezyGo.Payment.Domain.Managers;

public class SeatService : ISeatService
{
    private readonly ITripClient _tripClient;
    public SeatService(ITripClient tripClient)
    {
        _tripClient = tripClient;
    }
    public async Task ConfirmSeatsAsync(string Seats)
    {
        await _tripClient.ConfirmSeatsAsync(Seats);
    }
}
