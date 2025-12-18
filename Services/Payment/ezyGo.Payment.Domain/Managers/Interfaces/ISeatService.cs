namespace ezyGo.Payment.Domain.Managers.Interfaces;

public interface ISeatService
{
    Task ConfirmSeatsAsync(string Seats);
}
