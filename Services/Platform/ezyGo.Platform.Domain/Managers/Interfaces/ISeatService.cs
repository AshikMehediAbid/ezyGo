namespace ezyGo.Platform.Domain.Managers.Interfaces;

public interface ISeatService
{
    Task<bool> ReserveSeatAsync(int seatId);
    Task<bool> ReleaseSeatAsync(int seatId);
}
