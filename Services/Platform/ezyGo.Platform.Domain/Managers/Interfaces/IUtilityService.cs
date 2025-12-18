namespace ezyGo.Platform.Domain.Managers.Interfaces;

public interface IUtilityService
{
    Task<byte[]> GenerateTicketPdfAsync(string transactionId);
}
