namespace ezyGo.Core.ServiceClients.TripClient.Clients;

public interface ISeatClient
{
    Task UpdateBookedSeat(List<int> Seats);
}
