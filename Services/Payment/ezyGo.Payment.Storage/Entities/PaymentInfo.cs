namespace ezyGo.Payment.Storage.Entities;

public class PaymentInfo
{
    public int Id { get; set; }
    public string TransactionId { get; set; }
    public string PassengerName { get; set; }
    public string PassengerEmail { get; set; }
    public string PassengerPhone { get; set; }
    public string BusNumber { get; set; }
    public string SeatNumbers { get; set; }
    public string SeatNames { get; set; }
    public DateTime JourneyDate { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public int Fare { get; set; }
    public string Status { get; set; }
}