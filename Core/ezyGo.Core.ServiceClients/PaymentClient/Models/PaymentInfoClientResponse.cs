namespace ezyGo.Core.ServiceClients.PaymentClient.Models;

public class PaymentInfoClientResponse
{
    public int Id { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public string PassengerName { get; set; } = string.Empty;
    public string PassengerEmail { get; set; } = string.Empty;
    public string PassengerPhone { get; set; } = string.Empty;
    public string BusNumber { get; set; } = string.Empty;
    public string SeatNumbers { get; set; } = string.Empty;
    public string SeatNames { get; set; } = string.Empty;
    public DateTime JourneyDate { get; set; }
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public int Fare { get; set; }
    public string Status { get; set; } = string.Empty;
}

